using System.Collections.Generic;
using System.Windows.Controls;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.WP7.Resources;

namespace WallpaperPatterns.WP7.Views
{
    public partial class ShareView
    {
        public List<string> ShareSources = new List<string>
        {
            AppResources.ShareEmail,
            AppResources.ShareSocialNetwork,
            AppResources.ShareTextMessaging,
            AppResources.ShareClipboard
        };

        public ShareView()
        {
            InitializeComponent();
            ShareListBox.ItemsSource = ShareSources;
        }

        private void ShareListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ShareListBox.SelectedItem as string;
            
            if (selectedItem == null)
                return;

            FlurryWP8SDK.Api.LogEvent("Wallpaper.Share");

            var item = (ShareViewModel)ViewModel;
            if (selectedItem == AppResources.ShareEmail)
                ShareHelper.ShareViaEmail(item);
            else if (selectedItem == AppResources.ShareSocialNetwork)
                ShareHelper.ShareViaSocial(item);
            else if (selectedItem == AppResources.ShareTextMessaging)
                ShareHelper.ShareViaSms(item);
            else if (selectedItem == AppResources.ShareClipboard)
                ShareHelper.ShareViaClipBoard(item);

            ShareListBox.SelectedIndex = -1;
        }
    }
}