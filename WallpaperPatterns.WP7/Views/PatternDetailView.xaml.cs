﻿using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using Windows.Phone.System.UserProfile;
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

        private void ApplicationBarIconButton_Click_Download(object sender, EventArgs e)
        {
            var viewModel = (PatternDetailViewModel) ViewModel;
            string title = viewModel.Title;
            string uriString = viewModel.ImageUrl;

            int targetWidth = 480;
            int targetHeight = 800;
            
            WriteableBitmap writeableBitmap = GetBitmap(uriString, targetWidth, targetHeight);
            SaveImageToMediaLibrary(writeableBitmap, targetWidth, targetHeight, title);
        }

        private async void ApplicationBarIconButton_Click_Wallpaper(object sender, EventArgs e)
        {
            var viewModel = (PatternDetailViewModel)ViewModel;
            string title = viewModel.Title;
            string uriString = viewModel.ImageUrl;
            string fileName = title + ".jpeg";

            int targetWidth = 480;
            int targetHeight = 800;

            WriteableBitmap writeableBitmap = GetBitmap(uriString, targetWidth, targetHeight);
            SaveImageToIsolatedStorage(writeableBitmap, targetWidth, targetHeight, fileName);

            if (!LockScreenManager.IsProvidedByCurrentApplication)
            {
                LockScreenRequestResult result = await LockScreenManager.RequestAccessAsync();
                if (result == LockScreenRequestResult.Granted)
                {
                    SetAsWallpaper(fileName);
                }
            }
            else
            {
                SetAsWallpaper(fileName);
            }
        }

        private void SetAsWallpaper(string filename)
        {
            string realPath = "ms-appdata:///local/" + filename;
            LockScreen.SetImageUri(new Uri(realPath, UriKind.Absolute));
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
    }
}