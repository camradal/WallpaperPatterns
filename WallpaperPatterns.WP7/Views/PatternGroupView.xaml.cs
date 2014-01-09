﻿using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Phone.Controls;
using WallpaperPatterns.Core.Service;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.WP7.Resources;
using WallpaperPatterns.WP7.Utilities;

namespace WallpaperPatterns.WP7.Views
{
    public partial class PatternGroupView
    {
        public PatternGroupView()
        {
            InitializeComponent();
            
            int numberOfStarts = AppSettings.NumberOfStarts;
            AppSettings.NumberOfStarts = numberOfStarts + 1;

            ShowLoading(numberOfStarts);
            ShowReviewPane();
        }

        private void ShowLoading(int numberOfStarts)
        {
            if (numberOfStarts == 1)
            {
                GlobalLoading.Instance.LoadingText = Strings.MessagePleaseWait;
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
    }
}