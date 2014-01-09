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
            Strings.ShareEmail,
            Strings.ShareSocialNetwork,
            Strings.ShareTextMessaging,
            Strings.ShareClipboard
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

            var item = (ShareViewModel)ViewModel;
            if (selectedItem == Strings.ShareEmail)
                ShareHelper.ShareViaEmail(item);
            else if (selectedItem == Strings.ShareSocialNetwork)
                ShareHelper.ShareViaSocial(item);
            else if (selectedItem == Strings.ShareTextMessaging)
                ShareHelper.ShareViaSms(item);
            else if (selectedItem == Strings.ShareClipboard)
                ShareHelper.ShareViaClipBoard(item);

            ShareListBox.SelectedIndex = -1;
        }
    }
}