using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Service;
using WallpaperPatterns.Core.ViewModels;

namespace WallpaperPatterns.Store81
{
    public class IncrementalLoadingNewestViewModel : MvxViewModel
    {
        private readonly MvxSubscriptionToken _token;
        private readonly IPatternClient _client;
        private readonly IMvxMessenger _messenger;
        private IncrementalLoadingNew _items;

        public IncrementalLoadingNew Items
        {
            get { return _items; }
            set { _items = value; RaisePropertyChanged(() => Items); }
        }

        public IncrementalLoadingNewestViewModel(IPatternClient client, IMvxMessenger messenger)
        {
            _client = client;
            _messenger = messenger;
            _items = new IncrementalLoadingNew(client);
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