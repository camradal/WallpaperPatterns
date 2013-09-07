using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async void Load()
        {
            bool updatedHighlights = false;
            List<Pattern> items = await _client.Newest();
            foreach (var pattern in items)
            {
                Items.Add(pattern);
                if (!updatedHighlights)
                {
                    _messenger.Publish(new HighlightChangedMessage(this));
                    updatedHighlights = true;
                }
            }
        }
    }
}