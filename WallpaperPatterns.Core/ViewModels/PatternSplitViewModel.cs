using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Core.ViewModels
{
    public class PatternSplitViewModel : MvxViewModel
    {
        private readonly IPatternClient _client;
        private List<Pattern> _patterns = new List<Pattern>();
        private int _selectedItemIndex;
        private int _id;

        public PatternSplitViewModel(IPatternClient client)
        {
            _client = client;
            Load();
        }

        public void Init(int id)
        {
            _id = id;
        }

        public async void Load()
        {
            Patterns = await _client.Newest();
        }

        public List<Pattern> Patterns
        {
            get { return _patterns; }
            set { _patterns = value; RaisePropertyChanged(() => Patterns); }
        }

        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set { _selectedItemIndex = value; RaisePropertyChanged(() => SelectedItemIndex); }
        }

        public ICommand OpenDetailsCommand
        {
            get
            {
                return new MvxCommand<Pattern>(DoNavigate);
            }
        }

        private void DoNavigate(Pattern selectedItem)
        {
            ShowViewModel<PatternViewModel>(new { id = selectedItem.Id });
        }
    }
}