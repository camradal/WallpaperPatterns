using System.ComponentModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace WallpaperPatterns.Store81
{
    public class TileCanvas : Canvas, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(TileCanvas), new PropertyMetadata(null, ImageSourceChanged));

        private Size lastActualSize;
        private bool loading;

        public TileCanvas()
        {
            Loading = true;
            LayoutUpdated += OnLayoutUpdated;
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public bool Loading
        {
            get
            {
                return loading;
            }
            private set
            {
                loading = value;
                RaisePropertyChanged("Loading");
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
            ImageSource src = self.ImageSource;
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
            Loading = false;
            var image = (Image)sender;
            image.ImageOpened -= ImageOnImageOpened;
            image.ImageFailed -= ImageOnImageFailed;
            Children.Add(new TextBlock { Text = exceptionRoutedEventArgs.ErrorMessage, Foreground = new SolidColorBrush(Colors.Red) });
        }

        private void ImageOnImageOpened(object sender, RoutedEventArgs routedEventArgs)
        {
            Loading = false;
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

            int width = bmp.PixelWidth;
            int height = bmp.PixelHeight;

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

            if (Opacity < 1.0 && this.Resources.ContainsKey("FadeIn"))
            {
                var animation = this.Resources["FadeIn"] as Storyboard;
                if (animation != null && animation.GetCurrentState() != ClockState.Active)
                {
                    animation.Begin();
                }
            }

            if (Opacity < 1.0 && this.Resources.ContainsKey("FadeInLarge"))
            {
                var animation = this.Resources["FadeInLarge"] as Storyboard;
                if (animation != null && animation.GetCurrentState() != ClockState.Active)
                {
                    animation.Begin();
                }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
