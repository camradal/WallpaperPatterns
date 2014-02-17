using System.Windows;
using Microsoft.Phone.Tasks;
using WallpaperPatterns.WP7.Resources;

namespace WallpaperPatterns.WP7.Utilities
{
    public sealed class BuyThisAppTask
    {
        private const int numberOfStartsThreshold = 9;

        public void ShowAfterThreshold()
        {
            int starts = AppSettings.NumberOfStarts;
            if ((starts == numberOfStartsThreshold) && GetMessageBoxResult() == MessageBoxResult.OK)
            {
                try
                {
                    var task = new MarketplaceDetailTask { ContentIdentifier = "9d4e5f16-2e43-4dac-966b-5d12d1ccf5bb" };
                    task.Show();
                }
                catch
                {
                }
            }
        }

        private MessageBoxResult GetMessageBoxResult()
        {
            return MessageBox.Show(
                AppResources.MessageBoxBuyThisAppSummary,
                AppResources.MessageBoxBuyThisAppTitle,
                MessageBoxButton.OKCancel);
        }
    }
}