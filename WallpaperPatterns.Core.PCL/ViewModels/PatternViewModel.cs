using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.PCL.Service;

namespace WallpaperPatterns.Core.PCL.ViewModels
{
    public class PatternViewModel : MvxViewModel
    {
        private readonly IPatternClient _client;
        private List<Pattern> _patterns = new List<Pattern>();
        private Pattern _selectedItem;
        private int _selectedItemId;

        public Pattern SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged(() => SelectedItem); }
        }

        public List<Pattern> Patterns
        {
            get { return _patterns; }
            set { _patterns = value; RaisePropertyChanged(() => Patterns); }
        }

        public PatternViewModel(IPatternClient client)
        {
            _client = client;
        }

        public void Init(int id)
        {
            _selectedItemId = id;
            Load();
        }

        public async void Load()
        {
            Patterns = await _client.Newest();
            SelectedItem = Patterns.First(p => p.Id == _selectedItemId);
        }
    }
}