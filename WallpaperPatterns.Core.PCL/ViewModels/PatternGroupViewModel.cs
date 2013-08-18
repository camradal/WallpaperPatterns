using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace WallpaperPatterns.Core.PCL.ViewModels
{
    public class PatternGroup
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class PatternGroupViewModel : MvxViewModel
    {
        private List<PatternGroup> _groups = new List<PatternGroup>();

        public List<PatternGroup> Groups
        {
            get { return _groups; }
            set { _groups = value; RaisePropertyChanged(() => Groups); }
        }

        public PatternGroupViewModel()
        {
            Load();
        }

        private void Load()
        {
            _groups.Add(new PatternGroup { Id = 1, Title = "Newest" });
            _groups.Add(new PatternGroup { Id = 2, Title = "Top" });
            _groups.Add(new PatternGroup { Id = 3, Title = "Random" });
            _groups.Add(new PatternGroup { Id = 4, Title = "Popular" });
        }

        public ICommand NavigateToSplitViewCommand
        {
            get
            {
                return new MvxCommand<PatternGroup>(DoNavigate);
            }
        }

        private void DoNavigate(PatternGroup selectedItem)
        {
            ShowViewModel<PatternSplitViewModel>(new { id = selectedItem.Id });
        }
    }
}
