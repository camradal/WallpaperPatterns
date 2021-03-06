﻿using Windows.ApplicationModel.DataTransfer;
using Windows.UI.ApplicationSettings;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
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
using Windows.Networking.Connectivity;
using Windows.UI.Popups;

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
            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;

            CheckConnectivity();
        }

        private async System.Threading.Tasks.Task CheckConnectivity()
        {
            if (!IsInternet())
            {
                HighlightHubSection.Opacity = 0;
                MessageDialog dialog = new MessageDialog("Please connect to the internet to use the app", "No internet connection");
                var result = await dialog.ShowAsync();
                App.Current.Exit();
            }
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            Pattern pattern = ((PatternGroupViewModel)ViewModel).HighlightPattern;
            if (pattern != null)
            {
                DataRequest request = args.Request;
                request.Data.Properties.Title = pattern.Title + " Pattern";
                request.Data.SetWebLink(new Uri(pattern.Url, UriKind.Absolute));
            }
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var cmd = new SettingsCommand("Privacy Policy", "Privacy Policy", async command =>
            {

                var url = new Uri("http://www.dapperpanda.com/privacy-policy");
                await Windows.System.Launcher.LaunchUriAsync(url);
            });
            args.Request.ApplicationCommands.Add(cmd);
        }

        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
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

        private void Hub_OnSectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            if (e.Section == NewestSection)
                NavigateToNewest();
            else if (e.Section == TopSection)
                NavigateToTop();
            else if (e.Section == FavoritesSection)
                NavigateToFavorites();
        }

        private void NavigateToNewest()
        {
            var viewDispatcher = Mvx.Resolve<IMvxViewDispatcher>();
            viewDispatcher.ShowViewModel(new MvxViewModelRequest(typeof(IncrementalLoadingNewestViewModel), null, null, null));
        }

        private void NavigateToTop()
        {
            var viewDispatcher = Mvx.Resolve<IMvxViewDispatcher>();
            viewDispatcher.ShowViewModel(new MvxViewModelRequest(typeof(IncrementalLoadingTopViewModel), null, null, null));
        }
        
        private void NavigateToFavorites()
        {
            ((PatternGroupViewModel)ViewModel).NavigateToFavorites.Execute(null);
        }

        private void HighlightContainerGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var viewModel = ((PatternGroupViewModel)ViewModel);
            if (viewModel.HighlightPattern != null)
            {
                viewModel.NavigateToHighlight.Execute(null);
            }
        }

        private void ButtonRefresh_OnClick(object sender, RoutedEventArgs e)
        {
            ((PatternGroupViewModel)ViewModel).Refresh.Execute(null);
        }
    }
}
