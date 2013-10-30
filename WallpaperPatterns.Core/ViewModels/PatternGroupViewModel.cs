using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Messages;
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
        private Pattern _highlightPattern;
        private List<PatternGroup> _groups = new List<PatternGroup>();
        private MvxSubscriptionToken _token;

        public Pattern HighlightPattern
        {
            get { return _highlightPattern; }
            set { _highlightPattern = value; RaisePropertyChanged(() => HighlightPattern); }
        }

        public List<PatternGroup> Groups
        {
            get { return _groups; }
            set { _groups = value; RaisePropertyChanged(() => Groups); }
        }

        public NewestViewModel Newest { get; private set; }
        public TopViewModel Top { get; private set; }
        public FavoritesViewModel Favorites { get; private set; }

        public PatternGroupViewModel(IPatternClient client, IFavoritesService favoritesService, IMvxMessenger messenger)
        {
            Newest = new NewestViewModel(client, messenger);
            Top = new TopViewModel(client, messenger);
            Favorites = new FavoritesViewModel(favoritesService, messenger);

            _token = messenger.Subscribe<HighlightChangedMessage>(ServiceOnHighlightChanged);
            
            _groups.Add(new PatternGroup { Id = 1, Title = "Newest" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Top" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Favorites" });
        }

        private void ServiceOnHighlightChanged(HighlightChangedMessage obj)
        {
            HighlightPattern = Newest.Items.FirstOrDefault();
        }

        private async void Load()
        {
        }

        public ICommand NavigateToDetail
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<PatternDetailViewModel>( new { id = item.Id }));
            }
        }

        public ICommand NavigateToHighlight
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<PatternDetailViewModel>( new { id = HighlightPattern.Id }));
            }
        }

        public ICommand NavigateToNewest
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<NewestViewModel>());
            }
        }

        public ICommand NavigateToTop
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<TopViewModel>());
            }
        }
        
        public ICommand NavigateToFavorites
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<FavoritesViewModel>());
            }
        }
    }
}
