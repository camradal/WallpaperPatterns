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
        private readonly IMvxMessenger _messenger;
        private ObservableCollection<Pattern> _items = new ObservableCollection<Pattern>();

        public ObservableCollection<Pattern> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }
        
        public FavoritesViewModel(IFavoritesService favoritesService, IMvxMessenger messenger)
        {
            _favoritesService = favoritesService;
            _messenger = messenger;
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
            _messenger.Publish(new LoadingChangedMessage(this, true));
            try
            {
                List<Pattern> favorites = _favoritesService.All();
                foreach (var favorite in favorites)
                {
                    Items.Add(favorite);
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

        public ICommand NavigateToDetail
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<PatternDetailViewModel>(new { id = item.Id }));
            }
        }
    }
}