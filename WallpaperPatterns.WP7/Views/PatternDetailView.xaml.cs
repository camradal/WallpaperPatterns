﻿using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using Windows.Phone.System.UserProfile;
using WallpaperPatterns.Core.ViewModels;
using WallpaperPatterns.WP7.Resources;
using WallpaperPatterns.WP7.Utilities;

namespace WallpaperPatterns.WP7.Views
{
    public partial class PatternDetailView
    {
        private readonly object locker = new object();
        private readonly AwaitableCriticalSection critSection = new AwaitableCriticalSection();

        public PatternDetailView()
        {
            InitializeComponent();
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Text = AppResources.ApplicationButtonShare;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Text = AppResources.ApplicationButtonDownload;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).Text = AppResources.ApplicationButtonShare;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text = AppResources.ApplicationButtonFavorite;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = (PatternDetailViewModel)ViewModel;
            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsFavorite")
                {
                    EnableDisableFavoriteButton();
                }
            };
            viewModel.PropertyChanged += (sender, args) =>
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = true;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[3]).IsEnabled = true;
            };
        }

        private void TileCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            bool currentlyVisible = ApplicationBar.IsVisible;
            if (currentlyVisible)
            {
                ApplicationBar.IsVisible = false;
                TitlePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                ApplicationBar.IsVisible = true;
                TitlePanel.Visibility = Visibility.Visible;
            }
        }

        private void ApplicationBarIconButton_Click_Share(object sender, EventArgs e)
        {
            var viewModel = (PatternDetailViewModel)ViewModel;
            var pattern = viewModel.Pattern;
            ((PatternDetailViewModel)ViewModel).NavigateToShare.Execute(pattern);
        }
        
        private void ApplicationBarIconButton_Click_Unfavorite(object sender, EventArgs e)
        {
            lock (locker)
            {
                ((PatternDetailViewModel) ViewModel).RemoveFavorite.Execute(null);
                GlobalLoading.Instance.SetTimedText(AppResources.MessagePatternUnfavorite);
            }
        }

        private void ApplicationBarIconButton_Click_Favorite(object sender, EventArgs e)
        {
            lock (locker)
            {
                ((PatternDetailViewModel) ViewModel).AddFavorite.Execute(null);
                GlobalLoading.Instance.SetTimedText(AppResources.MessagePatternFavorite);
            }
        }

        private void ApplicationBarIconButton_Click_Download(object sender, EventArgs e)
        {
            lock (locker)
            {
                var viewModel = (PatternDetailViewModel) ViewModel;
                string title = viewModel.Title;
                string uriString = viewModel.ImageUrl;

                var size = ResolutionHelper.DisplayResolution;
                int targetWidth = (int)size.Width;
                int targetHeight = (int)size.Height;

                WriteableBitmap writeableBitmap = GetBitmap(uriString, targetWidth, targetHeight);
                SaveImageToMediaLibrary(writeableBitmap, targetWidth, targetHeight, title);
                GlobalLoading.Instance.SetTimedText(AppResources.MessagePatternDownloaded);

                FlurryWP8SDK.Api.LogEvent("Wallpaper.Download");
            }
        }

        private async void ApplicationBarIconButton_Click_Wallpaper(object sender, EventArgs e)
        {
            var viewModel = (PatternDetailViewModel)ViewModel;
            string title = viewModel.Title;
            string uriString = viewModel.ImageUrl;
            string fileName = StripInvalidChars(title) + ".jpeg";

            var size = ResolutionHelper.DisplayResolution;
            int targetWidth = (int)size.Width;
            int targetHeight = (int)size.Height;

            lock (locker)
            {
                WriteableBitmap writeableBitmap = GetBitmap(uriString, targetWidth, targetHeight);
                SaveImageToIsolatedStorage(writeableBitmap, targetWidth, targetHeight, fileName);
            }

            using (var section = await critSection.EnterAsync())
            {
                if (!LockScreenManager.IsProvidedByCurrentApplication)
                {
                    LockScreenRequestResult result = await LockScreenManager.RequestAccessAsync();
                    if (result == LockScreenRequestResult.Granted)
                    {
                        lock (locker)
                        {
                            SetAsWallpaper(fileName);
                        }
                    }
                }
                else
                {
                    lock (locker)
                    {
                        SetAsWallpaper(fileName);
                    }
                }
            }
        }

        private void EnableDisableFavoriteButton()
        {
            var viewModel = (PatternDetailViewModel)ViewModel;
            if (viewModel.IsFavorite)
            {
                AddUnfavoriteButton();
            }
            else if (((ApplicationBarIconButton)ApplicationBar.Buttons[3]).Text == AppResources.ApplicationButtonUnfavorite)
            {
                AddFavoriteButton();
            }
        }

        private void AddFavoriteButton()
        {
            var button = new ApplicationBarIconButton
            {
                IconUri = new Uri("/icons/appbar.favs.addto.rest.png", UriKind.Relative),
                Text = AppResources.ApplicationButtonFavorite,
            };
            button.Click += ApplicationBarIconButton_Click_Favorite;

            ApplicationBar.Buttons.RemoveAt(3);
            ApplicationBar.Buttons.Add(button);
            FlurryWP8SDK.Api.LogEvent("Wallpaper.AddFavorite");
        }

        private void AddUnfavoriteButton()
        {
            var button = new ApplicationBarIconButton
            {
                IconUri = new Uri("/icons/appbar.star.minus.png", UriKind.Relative),
                Text = AppResources.ApplicationButtonUnfavorite
            };
            button.Click += ApplicationBarIconButton_Click_Unfavorite;

            ApplicationBar.Buttons.RemoveAt(3);
            ApplicationBar.Buttons.Add(button);
            FlurryWP8SDK.Api.LogEvent("Wallpaper.RemoveFavorite");
        }

        private void SetAsWallpaper(string filename)
        {
            string realPath = "ms-appdata:///local/" + filename;
            LockScreen.SetImageUri(new Uri(realPath, UriKind.Absolute));
            GlobalLoading.Instance.SetTimedText(AppResources.MessagePatternOnLockscreen);
            FlurryWP8SDK.Api.LogEvent("Wallpaper.SetLockscreen");
        }

        private WriteableBitmap GetBitmap(string uriString, int targetWidth, int targetHeight)
        {
            var uriSource = new Uri(uriString, UriKind.Absolute);
            var imageSource = new BitmapImage(uriSource) { CreateOptions = BitmapCreateOptions.None };
            var canvas = GetCanvas(imageSource, targetWidth, targetHeight);
            var writeableBitmap = new WriteableBitmap(canvas, null);
            return writeableBitmap;
        }

        private TileCanvas GetCanvas(BitmapImage imageSource, int targetWidth, int targetHeight)
        {
            int width = imageSource.PixelWidth;
            int height = imageSource.PixelHeight;

            var canvas = new TileCanvas { Width = targetWidth, Height = targetHeight };
            for (int x = 0; x < targetWidth; x += width)
            {
                for (int y = 0; y < targetHeight; y += height)
                {
                    var image = new Image { Source = imageSource };
                    Canvas.SetLeft(image, x);
                    Canvas.SetTop(image, y);
                    canvas.Children.Add(image);
                }
            }
            canvas.Clip = new RectangleGeometry { Rect = new Rect(0, 0, targetWidth, targetHeight) };
            return canvas;
        }

        private static void SaveImageToMediaLibrary(WriteableBitmap bitmap, int targetWidth, int targetHeight, string title)
        {
            using (var stream = new MemoryStream())
            using (var mediaLibrary = new MediaLibrary())
            {
                if (bitmap.PixelHeight > 0)
                {
                    bitmap.SaveJpeg(stream, targetWidth, targetHeight, 0, 100);
                }
                stream.Seek(0, SeekOrigin.Begin);
                mediaLibrary.SavePicture(title, stream);
            }
        }

        private static void SaveImageToIsolatedStorage(WriteableBitmap bitmap, int targetWidth, int targetHeight, string fileName)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists(fileName))
                {
                    storage.DeleteFile(fileName);
                }

                using (IsolatedStorageFileStream stream = storage.CreateFile(fileName))
                {
                    bitmap.SaveJpeg(stream, targetWidth, targetHeight, 0, 100);
                }
            }
        }

        private string StripInvalidChars(string str)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            foreach (var c in invalid)
            {
                str = str.Replace(c.ToString(), "");
            }
            return str;
        }
    }
}