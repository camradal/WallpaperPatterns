using Cirrious.MvvmCross.Plugins.Messenger;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class ShareViewModel : PatternDetailViewModel
    {
        public ShareViewModel(IPatternClient client, IFavoritesService favoritesService, IImageService imageService,
            IMvxMessenger messenger) : base(client, favoritesService, imageService, messenger)
        {
        }
    }
}