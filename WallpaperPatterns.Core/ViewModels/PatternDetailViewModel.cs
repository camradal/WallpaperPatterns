using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Messages;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class PatternDetailViewModel : MvxViewModel
    {
        private readonly IPatternClient _client;
        private readonly IFavoritesService _favoritesService;
        private readonly IImageService _imageService;
        private readonly IMvxMessenger _messenger;

        private Pattern _pattern;
        private string _title;
        private string _byUserName;
        private string _imageUrl;

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(() => Title); }
        }

        public string ByUserName
        {
            get { return _byUserName; }
            set { _byUserName = value; RaisePropertyChanged(() => ByUserName); }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; RaisePropertyChanged(() => ImageUrl); }
        }

        public PatternDetailViewModel(IPatternClient client, IFavoritesService favoritesService, IImageService imageService, IMvxMessenger messenger)
        {
            _client = client;
            _favoritesService = favoritesService;
            _imageService = imageService;
            _messenger = messenger;
        }

        public void Init(int id)
        {
            Load(id);
        }

        public async void Load(int id)
        {
            _messenger.Publish(new LoadingChangedMessage(this, true));
            try
            {
                _pattern = await _client.Get(id);
                if (_pattern != null)
                {
                    Title = _pattern.Title;
                    ByUserName = _pattern.ByUserName;
                    ImageUrl = _pattern.ImageUrl;
                }
                else
                {
                    // TODO: error?
                }
            }
            finally
            {
                _messenger.Publish(new LoadingChangedMessage(this, false));                
            }
        }

        public ICommand AddFavorite
        {
            get
            {
                return new MvxCommand(() => _favoritesService.Insert(_pattern));
            }
        }
        
        public ICommand Download
        {
            get
            {
                return new MvxCommand(() => _imageService.Save("picture.jpg", new byte[] {}));
            }
        }
    }
}