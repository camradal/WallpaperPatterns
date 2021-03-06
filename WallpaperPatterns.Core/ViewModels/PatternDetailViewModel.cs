﻿using System.Windows.Input;
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
        private string _url;
        private bool _isLoading;

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

        public string Url
        {
            get { return _url; }
            set { _url = value; RaisePropertyChanged(() => Url); }
        }

        public bool IsFavorite
        {
            get { return _favoritesService.Contains(_pattern); }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
        }

        public Pattern Pattern
        {
            get { return _pattern; }
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
                    Url = _pattern.Url;
                    RaisePropertyChanged(() => IsFavorite);
                    RaisePropertyChanged(() => IsLoading);
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
                return new MvxCommand(() =>
                {
                    _favoritesService.Insert(_pattern);
                    RaisePropertyChanged(() => IsFavorite);
                });
            }
        }

        public ICommand RemoveFavorite
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _favoritesService.Delete(_pattern);
                    RaisePropertyChanged(() => IsFavorite);
                });
            }
        }

        public ICommand NavigateToShare
        {
            get
            {
                return new MvxCommand<Pattern>(item => ShowViewModel<ShareViewModel>(new { id = item.Id }));
            }
        }
    }
}