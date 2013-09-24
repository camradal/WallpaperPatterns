using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace WallpaperPatterns.Store81
{
    public class TileCanvas : Canvas
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(TileCanvas), new PropertyMetadata(null, ImageSourceChanged));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
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

        private void ImageOnImageFailed(object sender, ExceptionRoutedEventArgs exceptionRoutedEventArgs)
        {
            var image = (Image)sender;
            image.ImageOpened -= ImageOnImageOpened;
            image.ImageFailed -= ImageOnImageFailed;
            Children.Add(new TextBlock { Text = exceptionRoutedEventArgs.ErrorMessage, Foreground = new SolidColorBrush(Colors.Red) });
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
            Children.Clear();

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

            for (int x = 0; x < ActualWidth; x += width)
            {
                for (int y = 0; y < ActualHeight; y += height)
                {
                    var image = new Image { Source = ImageSource };
                    Canvas.SetLeft(image, x);
                    Canvas.SetTop(image, y);
                    Children.Add(image);
                }
            }
            Clip = new RectangleGeometry { Rect = new Rect(0, 0, ActualWidth, ActualHeight) };
            CacheMode = new BitmapCache();

            if (this.Resources.ContainsKey("FadeIn"))
            {
                var animation = this.Resources["FadeIn"] as Storyboard;
                if (animation != null)
                {
                    animation.Begin();
                }
            }
            
            if (this.Resources.ContainsKey("FadeInLarge"))
            {
                var animation = this.Resources["FadeInLarge"] as Storyboard;
                if (animation != null)
                {
                    animation.Begin();
                }
            }
        }
    }
}
