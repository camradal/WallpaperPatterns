using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class PatternGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class PatternGroupViewModel : MvxViewModel
    {
        private readonly IPatternClient _client;
        private readonly IFavoritesService _favoritesService;
        private List<PatternGroup> _groups = new List<PatternGroup>();
        private List<Pattern> _newest = new List<Pattern>();
        private List<Pattern> _top = new List<Pattern>();
        private List<Pattern> _favorites = new List<Pattern>();

        public List<PatternGroup> Groups
        {
            get { return _groups; }
            set { _groups = value; RaisePropertyChanged(() => Groups); }
        }

        public List<Pattern> Newest
        {
            get { return _newest; }
            set { _newest = value; RaisePropertyChanged(() => Newest); }
        }

        public List<Pattern> Top
        {
            get { return _top; }
            set { _top = value; RaisePropertyChanged(() => Top); }
        }

        public FavoritesViewModel Favorites { get; private set; }

        public PatternGroupViewModel(IPatternClient client, IFavoritesService favoritesService, IMvxMessenger messenger)
        {
            _client = client;
            _favoritesService = favoritesService;
            Load();
            Favorites = new FavoritesViewModel(favoritesService, messenger);
        }

        private async void Load()
        {
            _groups.Add(new PatternGroup { Id = 1, Title = "Newest" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Top" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Favorites" });

            Newest = await _client.Newest();
            Top = await _client.Top();
        }

        public ICommand NavigateToDetail
        {
            get
            {
                return new MvxCommand<Pattern>(DoNavigate);
            }
        }

        private void DoNavigate(Pattern selectedItem)
        {
            ShowViewModel<PatternDetailViewModel>(new { id = selectedItem.Id });
        }
    }
}
