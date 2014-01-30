using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using WallpaperPatterns.Core.Service;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.WP7.Resources;
using WallpaperPatterns.WP7.Utilities;

namespace WallpaperPatterns.WP7.Views
{
    public partial class PatternGroupView
    {
        public List<string> MenuSources = new List<string>
        {
            //Strings.MenuItemBuyAdFreeVersion,
            Strings.MenuItemRateThisApp,
            Strings.MenuItemMoreApps,
            Strings.MenuItemAbout
        };

        public PatternGroupView()
        {
            InitializeComponent();
            
            int numberOfStarts = AppSettings.NumberOfStarts;
            AppSettings.NumberOfStarts = numberOfStarts + 1;

            ShowLoading(numberOfStarts);
            ShowReviewPane();

            MenuListBox.ItemsSource = MenuSources;

            // ads
            AdBox.ErrorOccurred += AdBox_ErrorOccurred;
            AdBox.AdRefreshed += AdBox_AdRefreshed;
        }

        private void ShowLoading(int numberOfStarts)
        {
            if (numberOfStarts == 1)
            {
                GlobalLoading.Instance.SetTimedText(Strings.MessagePleaseWait);
            }
        }

        private void ShowReviewPane()
        {
            var rate = new ReviewThisAppTask();
            rate.ShowAfterThreshold();
        }

        private void ListBox_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var listBox = (LongListSelector)sender;
            var selectedItem = listBox.SelectedItem as Pattern;
            if (selectedItem == null)
                return;

            ((PatternGroupViewModel)ViewModel).NavigateToDetail.Execute(selectedItem);
        }

        private void NewListBox_OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var listBox = sender as LongListSelector;
            if (listBox == null) return;

            var items = listBox.ItemsSource as ObservableCollection<Pattern>;
            if (items == null) return;

            var currentItem = e.Container.Content as Pattern;
            if (currentItem == null) return;

            NewestViewModel viewModel = ((PatternGroupViewModel)ViewModel).Newest;
            if (currentItem.Equals(viewModel.Items.Last()))
            {
                int offset = viewModel.Items.Count;
                viewModel.Load(offset);
            }
        }

        private void TopListBox_OnItemRealized(object sender, ItemRealizationEventArgs e)
        {
            var listBox = sender as LongListSelector;
            if (listBox == null) return;

            var items = listBox.ItemsSource as ObservableCollection<Pattern>;
            if (items == null) return;

            var currentItem = e.Container.Content as Pattern;
            if (currentItem == null) return;

            TopViewModel viewModel = ((PatternGroupViewModel)ViewModel).Top;
            if (currentItem.Equals(viewModel.Items.Last()))
            {
                int offset = viewModel.Items.Count;
                viewModel.Load(offset);
            }
        }

        private void MenuListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = MenuListBox.SelectedItem as string;

            if (selectedItem == null)
                return;

            if (selectedItem == Strings.MenuItemBuyAdFreeVersion)
            {
                // TODO
            }
            else if (selectedItem == Strings.MenuItemRateThisApp)
                RateThisApp();
            else if (selectedItem == Strings.MenuItemMoreApps)
                MoreApps();
            else if (selectedItem == Strings.MenuItemAbout)
                ((PatternGroupViewModel) ViewModel).NavigateToAbout.Execute(null);

            MenuListBox.SelectedIndex = -1;
        }

        private void MoreApps()
        {
            try
            {
                var task = new MarketplaceSearchTask();
                task.ContentType = MarketplaceContentType.Applications;
                task.SearchTerms = "Dapper Panda";
                task.Show();
            }
            catch
            {
                // prevent exceptions from double-click
            }
        }

        private void RateThisApp()
        {
            try
            {
                var task = new MarketplaceReviewTask();
                task.Show();
            }
            catch
            {
                // prevent exceptions from double-click
            }
        }

        #region Ads

        void AdBox_AdRefreshed(object sender, EventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                AdDuplexAdControl.Visibility = Visibility.Collapsed;
                AdBox.Visibility = Visibility.Visible;
            });
        }

        void AdBox_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                AdBox.Visibility = Visibility.Collapsed;
                AdDuplexAdControl.Visibility = Visibility.Visible;
            });
        }

        #endregion
    }
}