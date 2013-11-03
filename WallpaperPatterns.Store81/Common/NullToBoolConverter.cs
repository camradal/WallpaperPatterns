using System;
using Windows.UI.Xaml.Data;

namespace WallpaperPatterns.Store81.Common
{
    public sealed class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value == null).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}