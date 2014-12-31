// <file>FlickrAccount.cs</author>
// <author>Stephen Hough</author>
// <date>12-17-2014</date>
// <summary>This class represents a Flickr account with a list of pictures</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HPHwFlickrViewer.Model
{
    class FlickrAccount : PropertyObservingModel
    {
        // This demo App has one and only one Flickr account
        private static FlickrAccount account;
        public static FlickrAccount ActiveAccount
        {
            get
            {
                if (account == null)
                {
                    account = new FlickrAccount();
                }

                return account;
            }
        }

        public FlickrAccount()
        {
            stream = new Model.FlickrStream(accountUrl);
        }

        // Load the list of photos
        public void LoadAccountInfo()
        {
            // Get notified when the load is completed
            stream.PropertyChanged += new PropertyChangedEventHandler(CurrentStream_PropertyChanged);
            stream.Load();
        }

        // Hard-code for demo
        private string accountUrl = "https://api.flickr.com/services/feeds/photos_public.gne?format=json&id=67710869@N08";

        // The account has a stream (list of photos)
        private Model.FlickrStream stream = null;
        public Model.FlickrStream CurrentStream
        {
            get
            {
                return stream;
            }
        }

        public void CurrentStream_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            // Notify listeners that the current Flickr stream has changed.
            NotifyPropertyChanged("CurrentStream");
        }
    }
}
