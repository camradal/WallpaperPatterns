using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
            // TODO: dispatch command
        }

        private void NewListBox_Link(object sender, LinkUnlinkEventArgs e)
        {
            // TODO: load more
        }
    }
}