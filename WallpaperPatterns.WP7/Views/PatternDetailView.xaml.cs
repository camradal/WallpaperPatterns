using System;
using System.Collections.Generic;
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
    public partial class PatternDetailView
    {
        public PatternDetailView()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click_Share(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ApplicationBarIconButton_Click_Favorite(object sender, EventArgs e)
        {
            ((PatternDetailViewModel)ViewModel).AddFavorite.Execute(null);
        }

        private void ApplicationBarMenuItem_OnClick_OpenInIE(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}