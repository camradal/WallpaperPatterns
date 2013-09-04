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
        private ObservableCollection<Pattern> _items = new ObservableCollection<Pattern>();

        public ObservableCollection<Pattern> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }
        
        public FavoritesViewModel(IFavoritesService favoritesService, IMvxMessenger messenger)
        {
            _favoritesService = favoritesService;
            _token = messenger.Subscribe<FavoritesChangedMessage>(ServiceOnFavoritesSessionsChanged);

            Load();
        }

        private void ServiceOnFavoritesSessionsChanged(FavoritesChangedMessage message)
        {
            Items.Clear();
            Load();
        }

        private void Load()
        {
            List<Pattern> favorites = _favoritesService.All();
            foreach (var favorite in favorites)
            {
                Items.Add(favorite);
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