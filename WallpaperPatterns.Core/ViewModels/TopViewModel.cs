using System.Collections.Generic;
using System.Collections.ObjectModel;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class TopViewModel: MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;
        private readonly IPatternClient _client;
        private ObservableCollection<Pattern> _items = new ObservableCollection<Pattern>();

        public ObservableCollection<Pattern> Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }
        
        public TopViewModel(IPatternClient client, IMvxMessenger messenger)
        {
            _client = client;
            Load();
        }

        public async void Load()
        {
            List<Pattern> items = await _client.Top();
            foreach (var pattern in items)
            {
                Items.Add(pattern);
            }
        }
    }
}