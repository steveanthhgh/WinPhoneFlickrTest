// <file>MainPage.cs</author>
// <author>Stephen Hough</author>
// <date>12-17-2014</date>
// <summary>This class has the code for the Main Page view</summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace HPHwFlickrViewer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            // Create the view model and use as Data Context (for Data Binding)
            vm = new ViewModels.MainPageViewModel();
            DataContext = vm;
        }

        private ViewModels.MainPageViewModel vm;
    }
}