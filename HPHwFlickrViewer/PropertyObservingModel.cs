// <file>PropertyObservingModel.cs</author>
// <author>Stephen Hough</author>
// <date>12-17-2014</date>
// <summary>Base class for classes that have observable propertiesr</summary>

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HPHwFlickrViewer
{
    public class PropertyObservingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String propertyName)
        {
            System.Diagnostics.Debug.WriteLine("NotifyPropertyChanged : " + propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
