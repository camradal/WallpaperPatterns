using Cirrious.MvvmCross.WindowsStore.Views;
using WallpaperPatterns.Core.Service;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.Store81.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=286574

namespace WallpaperPatterns.Store81.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class PatternGroupView : MvxStorePage
    {
        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public PatternGroupView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }

        private void Grid_OnLoaded(object sender, RoutedEventArgs e)
        {
            var grid = (Grid)sender;
            grid.Height = HighlightHubSection.ActualHeight;
            grid.Width = HighlightHubSection.ActualWidth;
        }

        private void ItemListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedItem = (Pattern)e.ClickedItem;
            if (selectedItem == null)
                return;

            ((PatternGroupViewModel)ViewModel).NavigateToDetail.Execute(selectedItem);
        }
    }
}
