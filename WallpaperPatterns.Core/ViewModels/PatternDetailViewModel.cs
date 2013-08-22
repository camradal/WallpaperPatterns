using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class PatternDetailViewModel : MvxViewModel
    {
        private readonly IPatternClient _client;

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

        public PatternDetailViewModel(IPatternClient client)
        {
            _client = client;
        }

        public void Init(int id)
        {
            Load(id);
        }

        public async void Load(int id)
        {
            Pattern pattern = await _client.Get(id);
            if (pattern != null)
            {
                Title = pattern.Title;
                ByUserName = pattern.ByUserName;
                ImageUrl = pattern.ImageUrl;
            }
            else
            {
                // TODO: error?
            }
        }
    }
}