using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WallpaperPatterns.WP7
{
    public class TileCanvas : Canvas
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource",
            typeof(ImageSource),
            typeof(TileCanvas),
            new PropertyMetadata(null, ImageSourceChanged));

        private Size lastActualSize;

        public TileCanvas()
        {
            LayoutUpdated += OnLayoutUpdated;
        }

        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
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

        private static void ImageSourceChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            TileCanvas self = (TileCanvas)o;
            var src = self.ImageSource;
            if (src != null)
            {
                var image = new Image { Source = src };
                image.ImageOpened += self.ImageOnImageOpened;
                image.ImageFailed += self.ImageOnImageFailed;

                //add it to the visual tree to kick off ImageOpened
                self.Children.Add(image);
            }
        }

        private void ImageOnImageFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            var image = (Image)sender;
            image.ImageOpened -= ImageOnImageOpened;
            image.ImageFailed -= ImageOnImageFailed;
            Children.Add(
                new TextBlock
                {
                    Text = exceptionRoutedEventArgs.ErrorException.Message,
                    Foreground = new SolidColorBrush(Colors.Red)
                });
        }

        private void ImageOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            var image = (Image)sender;
            image.ImageOpened -= ImageOnImageOpened;
            image.ImageFailed -= ImageOnImageFailed;
            Children.Clear();
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

            if (ActualWidth == 0 || ActualHeight == 0)
            {
                return;
            }

            double currentTiledWidth = -1;
            double currentTiledHeight = -1;
            foreach (var child in this.Children.ToList().Where(c => c is Image).Cast<Image>())
            {
                int childTop = (int)Canvas.GetTop(child);
                int childLeft = (int)Canvas.GetLeft(child);
                if (childLeft >= ActualWidth)
                {
                    Children.Remove(child);
                }
                else if (childTop >= ActualHeight)
                {
                    Children.Remove(child);
                }
                else
                {
                    if (childLeft > currentTiledWidth) currentTiledWidth = childLeft;
                    if (childTop > currentTiledHeight) currentTiledHeight = childTop;
                }
            }

            for (int x = 0; x < ActualWidth; x += width)
            {
                for (int y = 0; y < ActualHeight; y += height)
                {
                    if (x > currentTiledWidth || y > currentTiledHeight)
                    {
                        var image = new Image { Source = ImageSource };
                        Canvas.SetLeft(image, x);
                        Canvas.SetTop(image, y);
                        Children.Add(image);
                    }
                }
            }
            Clip = new RectangleGeometry { Rect = new Rect(0, 0, ActualWidth, ActualHeight) };
        }
    }
}