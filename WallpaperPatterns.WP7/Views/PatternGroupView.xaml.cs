using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WallpaperPatterns.Core.Service;
using WallpaperPatterns.Core.ViewModels;

namespace WallpaperPatterns.WP7.Views
{
    public partial class PatternGroupView
    {
        public PatternGroupView()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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