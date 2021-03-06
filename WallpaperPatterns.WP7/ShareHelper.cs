using System;
using System.Windows;
using Microsoft.Phone.Tasks;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.WP7.Resources;

namespace WallpaperPatterns.WP7
{
    static internal class ShareHelper
    {
        internal static void ShareViaEmail(PatternDetailViewModel model)
        {
            try
            {
                var task = new EmailComposeTask
                {
                    Subject = model.Title,
                    Body = model.Title + "\n\n" + model.Url
                };
                task.Show();
            }
            catch (Exception)
            {
                // fast-clicking can result in exception, so we just handle it
            }
        }

        internal static void ShareViaSocial(PatternDetailViewModel model)
        {
            try
            {
                var task = new ShareLinkTask()
                {
                    Title = model.Title,
                    Message = model.Title,
                    LinkUri = new Uri(model.Url, UriKind.Absolute)
                };
                task.Show();
            }
            catch (Exception)
            {
                // fast-clicking can result in exception, so we just handle it
            }
        }

        internal static void ShareViaSms(PatternDetailViewModel model)
        {
            try
            {
                var task = new SmsComposeTask()
                {
                    Body = model.Title + "\n" + model.Url
                };
                task.Show();
            }
            catch (Exception)
            {
                // fast-clicking can result in exception, so we just handle it
            }
        }

        internal static void ShareViaClipBoard(PatternDetailViewModel model)
        {
            string text = model.Title + "\n" + model.Url;
            if (MessageBox.Show(text, AppResources.MessageToClipboard, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            { 
                Clipboard.SetText(text);
            }
        }
    }
}
