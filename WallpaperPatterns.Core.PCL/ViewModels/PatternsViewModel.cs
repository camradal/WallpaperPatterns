using System.Collections.Generic;
using Cirrious.MvvmCross.ViewModels;

namespace WallpaperPatterns.Core.PCL.ViewModels
{
    public class PatternsViewModel : MvxViewModel
    {
        private readonly PatternClient _client = new PatternClient();
        private List<Pattern> _patterns = new List<Pattern>();

        public PatternsViewModel()
        {
            Load();
        }

        public async void Load()
        {
            Patterns = await _client.GetNewest();
        }

        public List<Pattern> Patterns
        {
            get { return _patterns; }
            set { _patterns = value; RaisePropertyChanged(() => Patterns); }
        }
    }
}