using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;

namespace WallpaperPatterns.Core.Net45.ViewModels
{
    public class NewItem
    {
        public string Version { get; set; }
        public string Description { get; set; }
    }

    public class AboutViewModel : MvxViewModel
    {
        public List<NewItem> NewItems { get; set; }
    }
}
