using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Messages;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels

{
    public class FavoritesViewModel: MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;
        private readonly IFavoritesService _favoritesService;
        private ObservableCollection<Pattern> _favorites = new ObservableCollection<Pattern>();

        public ObservableCollection<Pattern> Favorites
        {
            get { return _favorites; }
            set { _favorites = value; RaisePropertyChanged(() => Favorites); }
        }
        
        public FavoritesViewModel(IFavoritesService favoritesService, IMvxMessenger messenger)
        {
            _favoritesService = favoritesService;
            _token = messenger.Subscribe<FavoritesChangedMessage>(ServiceOnFavoritesSessionsChanged);

            PopulateFavorites();
        }

        private void ServiceOnFavoritesSessionsChanged(FavoritesChangedMessage message)
        {
            Favorites.Clear();
            PopulateFavorites();
        }

        private void PopulateFavorites()
        {
            List<Pattern> favorites = _favoritesService.All();
            foreach (var favorite in favorites)
            {
                Favorites.Add(favorite);
            }
        }

        public ICommand AddFavorite
        {
            get
            {
                return new MvxCommand<Pattern>(selectedItem => _favoritesService.Insert(selectedItem));
            }
        }

        public ICommand RemoveFavorite
        {
            get
            {
                return new MvxCommand<Pattern>(selectedItem => _favoritesService.Delete(selectedItem));
            }
        }
    }
}