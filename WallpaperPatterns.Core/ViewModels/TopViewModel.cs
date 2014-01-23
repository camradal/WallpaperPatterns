using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Messages;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class TopViewModel: MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;
        private readonly IPatternClient _client;
        private readonly IMvxMessenger _messenger;
        private ObservableCollection<Pattern> _items = new ObservableCollection<Pattern>();
        private bool _isLoading;
        private volatile int _lastOffset = -1;

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
        
        public TopViewModel(IPatternClient client, IMvxMessenger messenger)
        {
            _client = client;
            _messenger = messenger;
            Load();
        }

        public async void Load(int offset = 0)
        {
            if (_lastOffset == offset || IsLoading)
                return;
            
            _lastOffset = offset;
            IsLoading = true;

            _messenger.Publish(new LoadingChangedMessage(this, true));
            try
            {
                List<Pattern> items = await _client.Top(offset);
                foreach (var pattern in items)
                {
                    Items.Add(pattern);
                }
            }
            finally
            {
                IsLoading = false;
                _messenger.Publish(new LoadingChangedMessage(this, false));
            }
        }

        public void Refresh()
        {
            if (!IsLoading)
            {
                _lastOffset = -1;
                Items.Clear();
                Load();
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