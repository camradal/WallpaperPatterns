using System.Collections.Generic;
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
        private List<PatternGroup> _groups = new List<PatternGroup>();

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
            
            _groups.Add(new PatternGroup { Id = 1, Title = "Newest" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Top" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Favorites" });
        }

        private async void Load()
        {
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
