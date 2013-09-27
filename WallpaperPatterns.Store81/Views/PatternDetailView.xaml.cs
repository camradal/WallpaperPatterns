using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media.Imaging;
using Cirrious.MvvmCross.WindowsStore.Views;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.Store81.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace WallpaperPatterns.Store81.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PatternDetailView : MvxStorePage
    {

        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public PatternDetailView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private async void ButtonDownload_OnClick(object sender, RoutedEventArgs e)
        {
            string title = GetPictureTitle();

            StorageLibrary pictures = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            StorageFolder folder = await pictures.SaveFolder.CreateFolderAsync("Wallpaper Patterns", CreationCollisionOption.OpenIfExists);
            StorageFile file = await folder.CreateFileAsync(title, CreationCollisionOption.ReplaceExisting);
            
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await SaveImageToStream(stream);
            }

            string text = string.Format("Downloaded image {0} to Pictures gallery", title);
            Notify(text);

        }

        private async void ButtonSetLockScreen_OnClick(object sender, RoutedEventArgs e)
        {
            string title = GetPictureTitle();

            using (var stream = new InMemoryRandomAccessStream())
            {
                await SaveImageToStream(stream);
                await Windows.System.UserProfile.LockScreen.SetImageStreamAsync(stream);
            }

            string text = string.Format("Set image {0} as your lockscreen", title);
            Notify(text);
        }

        private string GetPictureTitle()
        {
            var model = (PatternDetailViewModel)this.ViewModel;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            string title = invalid.Aggregate(model.Title, (current, c) => current.Replace(c.ToString(), ""));
            return title + ".jpg";
        }

        private async Task SaveImageToStream(IRandomAccessStream stream)
        {
            // render bitmap
            var bitmap = new RenderTargetBitmap();
            await bitmap.RenderAsync(TileCanvas);
            IBuffer pixels = await bitmap.GetPixelsAsync();
            byte[] bytes = pixels.ToArray();

            // encode bitmap
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
            encoder.BitmapTransform.Bounds = new BitmapBounds
            {
                Width = (uint)Window.Current.Bounds.Width,
                Height = (uint)Window.Current.Bounds.Height
            };
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96, 96, bytes);
            await encoder.FlushAsync();
            await stream.FlushAsync();
        }

        private void Notify(string text)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            var element = template.GetElementsByTagName("text")[0];
            element.AppendChild(template.CreateTextNode(text));

            var toast = new ToastNotification(template);
            notifier.Show(toast);  
        }
    }
}
