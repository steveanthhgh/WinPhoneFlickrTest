// <file>MainPageViewModel.cs</author>
// <author>Stephen Hough</author>
// <date>12-17-2014</date>
// <summary>This class is the View Model for the MainPage view</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HPHwFlickrViewer.Model;
using System.ComponentModel;

namespace HPHwFlickrViewer.ViewModels
{
    public class MainPageViewModel : PropertyObservingModel
    {
        public MainPageViewModel()
        {
            // Need to know when the model updates the current Flickr stream
            FlickrAccount.ActiveAccount.CurrentStream.PropertyChanged += CurrentStream_PropertyChanged;
        }

        // An ObservableCollection (for DataBinding) of items in the Flickt Stream
        public ObservableCollection<FlickrEntry> CurrentFlickrItems
        {
            get
            {
                if (FlickrAccount.ActiveAccount.CurrentStream != null && 
                            FlickrAccount.ActiveAccount.CurrentStream.StreamInfo != null && 
                            FlickrAccount.ActiveAccount.CurrentStream.StreamInfo.Items != null)
                    return new ObservableCollection<FlickrEntry>(FlickrAccount.ActiveAccount.CurrentStream.StreamInfo.Items);
                else
                    return  new ObservableCollection<FlickrEntry>();
            }
        }

        // Return true if the Flickr stream is not loaded
        public bool WaitingForFlickrStreamInfo
        {
            get
            {
                if (FlickrAccount.ActiveAccount.CurrentStream != null && 
                            FlickrAccount.ActiveAccount.CurrentStream.StreamInfo != null && 
                            FlickrAccount.ActiveAccount.CurrentStream.StreamInfo.Items != null)
                    return FlickrAccount.ActiveAccount.CurrentStream.State != FlickrStreamDataState.Loaded;
                else
                    return true;
            }
        }

        public async void CurrentStream_PropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            // Handle changes in the model data (Flickr Stream) by notifying the view of changes.
            if (FlickrAccount.ActiveAccount.CurrentStream.State == FlickrStreamDataState.LoadError)
            {
                // Have error handling here!
            }

            var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged("CurrentFlickrItems");
                NotifyPropertyChanged("WaitingForFlickrStreamInfo");
            });
        }

    }
}
