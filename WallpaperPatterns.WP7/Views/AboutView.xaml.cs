using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Tasks;
using WallpaperPatterns.Core.Net45.ViewModels;
using WallpaperPatterns.WP7.Resources;

namespace WallpaperPatterns.WP7.Views
{
    public partial class AboutView
    {
        public List<NewItem> NewItems
        {
            get
            {
                return new List<NewItem>()
                {
                    new NewItem
                    {
                        Version = "",
                        Description = "We read all emails, please drop us a line with any suggestions."
                    },
                    new NewItem
                    {
                        Version = "1.1",
                        Description =
                            "- HD and multi-resolution support\n" + 
                            "- Tap a pattern to view it in fullscreen\n" + 
                            "- Russian localization\n" + 
                            "- Bug fixes"
                    },
                    new NewItem
                    {
                        Version = "1.0",
                        Description =
                            "- Initial release"
                    }
                };
            }
        }

        public AboutView()
        {
            InitializeComponent();
            ReadVersionFromManifest();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ((AboutViewModel)ViewModel).NewItems = NewItems;
        }

        private void feedbackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailComposeTask task = new EmailComposeTask();
                task.Subject = AppResources.FeedbackOn;
                task.Body = AppResources.FeedbackTemplate;
                task.To = AppResources.ContactEmail;
                task.Show();
            }
            catch
            {
                // prevent exceptions from double-click
            }
        }

        private void rateButton_Click(object sender, RoutedEventArgs e)
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

        private void ReadVersionFromManifest()
        {
            versionText.Text = "1.0.0";
        }
    }
}