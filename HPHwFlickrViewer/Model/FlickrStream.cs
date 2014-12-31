// <file>FlickrStream.cs</author>
// <author>Stephen Hough</author>
// <date>12-17-2014</date>
// <summary>This class represents a Flick stream and contains the information about the photos therein. It loads the information from the Flickr server</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace HPHwFlickrViewer.Model
{
    [DataContract]
    public class FlickrMediaItem
    {
        [DataMember(Name = "m")]
        public string MediaUrl { get; set; }
    }

    [DataContract]
    public class FlickrEntry
    {
        [DataMember(Name = "title")]  
        public string Title { get; set; }
        [DataMember(Name = "link")]  
        public string Link { get; set; }
        [DataMember(Name = "media")]
        public FlickrMediaItem Media { get; set; }
        [DataMember(Name = "date_taken")]
        public string DateTaken { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "published")]
        public string Published { get; set; }
        [DataMember(Name = "author")]
        public string Author { get; set; }
        [DataMember(Name = "author_id")]
        public string AuthorId { get; set; }
        [DataMember(Name = "tags")]
        public string Tags { get; set; }
    }

    [DataContract]
    public class FlickrStreamInfo
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "link")]
        public string Link { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "modified")]
        public string Modified { get; set; }
        [DataMember(Name = "generator")]
        public string Generator { get; set; }
        [DataMember(Name = "items")]
        public IList<FlickrEntry> Items { get; set; }
    }

    public enum FlickrStreamDataState
    {
        NotLoaded,
        Loading,
        Loaded,
        LoadError
    };

    public class FlickrStream : PropertyObservingModel
    {
        private string accountUrl = "";
        private string flickrFeedTag = "jsonFlickrFeed";
        private FlickrStreamInfo streamInfo = null;
       
        public FlickrStream(string url)
        {
            accountUrl = url;
        }

        public void Load()
        {
            if (State != FlickrStreamDataState.Loading)
            {
                State = FlickrStreamDataState.Loading;
                loadFailMsg = "";

                HttpWebRequest request = WebRequest.CreateHttp(accountUrl);
                request.Method = "POST";

                //we first obtain an input stream to which to write the body of the HTTP POST
                request.BeginGetRequestStream((IAsyncResult result) =>
                {
                    HttpWebRequest preq = result.AsyncState as HttpWebRequest;
                    if (preq == null)
                    {
                        State = FlickrStreamDataState.LoadError;
                        loadFailMsg = "BeginGetRequestStream did not provide a HttpWebRequest";
                    }
                    else
                    {
                        //we can then finalize the request...
                        preq.BeginGetResponse((IAsyncResult final_result) =>
                        {
                            HttpWebRequest req = final_result.AsyncState as HttpWebRequest;
                            if (req != null)
                            {
                                try
                                {
                                    //we call the success callback as long as we get a response stream
                                    WebResponse response = req.EndGetResponse(final_result);

                                    using (var reader = new StreamReader(response.GetResponseStream()))
                                    {
                                        string data = reader.ReadToEnd();

                                        if (data.StartsWith(flickrFeedTag))
                                        {
                                            // Remove tag and open and closing brackets
                                            string temp = data.Substring(flickrFeedTag.Length);
                                            string jsonString = temp.Substring(1, temp.Length - 2);
                                            var serializer = new DataContractJsonSerializer(typeof(FlickrStreamInfo));
                                            MemoryStream ms = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(jsonString));
                                            streamInfo = (FlickrStreamInfo)serializer.ReadObject(ms);

                                            State = FlickrStreamDataState.Loaded;
                                        }
                                    }
                                }
                                catch (WebException ex)
                                {
                                    State = FlickrStreamDataState.LoadError;
                                    // set the failure message
                                    loadFailMsg = ex.Message;
                                }
                            }
                        }, preq);
                    }
                }, request);
            }
        }

        private string loadFailMsg = "";
        private FlickrStreamDataState loadState = FlickrStreamDataState.NotLoaded;
        public FlickrStreamDataState State
        {
            get
            {
                return loadState;
            }
            set
            {
                loadState = value;
                NotifyPropertyChanged("State");
                NotifyPropertyChanged("CurrentStream");
            }
        }

        public FlickrStreamInfo StreamInfo
        {
            get
            {
                return streamInfo;
            }
        }
    }
}
