using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Messages;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class NewestViewModel: MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;
        private readonly IPatternClient _client;
        private readonly IMvxMessenger _messenger;
        private ObservableCollection<Pattern> _items = new ObservableCollection<Pattern>();
        private bool _updatedHighlights;
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; RaisePropertyChanged(() => IsLoading); }
        }

        public ObservableCollection<Pattern> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }
        
        public NewestViewModel(IPatternClient client, IMvxMessenger messenger)
        {
            _client = client;
            _messenger = messenger;
            Load();
        }

        public async void Load(int offset = 0)
        {
            IsLoading = true;
            _messenger.Publish(new LoadingChangedMessage(this, true));
            try
            {
                List<Pattern> items = await _client.Newest(offset);
                foreach (var pattern in items)
                {
                    Items.Add(pattern);
                    if (!_updatedHighlights)
                    {
                        _messenger.Publish(new HighlightChangedMessage(this));
                        _updatedHighlights = true;
                    }
                }
            }
            finally
            {
                IsLoading = false;
                _messenger.Publish(new LoadingChangedMessage(this, false));                
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