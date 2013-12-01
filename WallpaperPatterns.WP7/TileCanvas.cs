using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace WallpaperPatterns.WP7
{
    public class TileCanvas : Canvas
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(TileCanvas), new PropertyMetadata(null, ImageSourceChanged));
        private Size lastActualSize;

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public TileCanvas()
        {
            LayoutUpdated += OnLayoutUpdated;
        }

        private static void ImageSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            TileCanvas self = (TileCanvas)o;
            var src = self.ImageSource;
            if (src != null)
            {
                self.Opacity = 0;
                self.Children.Clear();
                var image = new Image { Source = src };
                image.ImageOpened += self.ImageOnImageOpened;
                image.ImageFailed += self.ImageOnImageFailed;

                //add it to the visual tree to kick off ImageOpened
                self.Children.Add(image);
            }
        }

        private void OnLayoutUpdated(object sender, object o)
        {
            var newSize = new Size(ActualWidth, ActualHeight);
            if (lastActualSize != newSize)
            {
                lastActualSize = newSize;
                Rebuild();
            }
        }

        private void ImageOnImageFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            var image = (Image)sender;
            image.ImageOpened -= ImageOnImageOpened;
            image.ImageFailed -= ImageOnImageFailed;
            Children.Add(new TextBlock { Text = exceptionRoutedEventArgs.ErrorException.Message, Foreground = new SolidColorBrush(Colors.Red) });
        }

        private void ImageOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            var image = (Image)sender;
            image.ImageOpened -= ImageOnImageOpened;
            image.ImageFailed -= ImageOnImageFailed;
            Rebuild();
        }

        private void Rebuild()
        {
            var bmp = ImageSource as BitmapSource;
            if (bmp == null)
            {
                return;
            }

            var width = bmp.PixelWidth;
            var height = bmp.PixelHeight;

            if (width == 0 || height == 0)
            {
                return;
            }

            // first image element has already been added
            bool first = true;
            for (int x = 0; x < ActualWidth; x += width)
            {
                for (int y = 0; y < ActualHeight; y += height)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }

                    var image = new Image { Source = ImageSource };
                    Canvas.SetLeft(image, x);
                    Canvas.SetTop(image, y);
                    Children.Add(image);
                }
            }
            Clip = new RectangleGeometry { Rect = new Rect(0, 0, ActualWidth, ActualHeight) };
            CacheMode = new BitmapCache();

            if (Opacity < 1.0 && this.Resources.Contains("FadeIn"))
            {
                var animation = this.Resources["FadeIn"] as Storyboard;
                if (animation != null)
                {
                    animation.Begin();
                }
            }
        }
    }
}