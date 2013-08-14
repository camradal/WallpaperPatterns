using Cirrious.MvvmCross.ViewModels;
using WallpaperPatterns.Core.PCL.Service;

namespace WallpaperPatterns.Core.PCL.ViewModels
{
    public class PatternDetailViewModel : MvxViewModel
    {
        private readonly IPatternClient _client;

        public PatternDetailViewModel(IPatternClient client)
        {
            _client = client;
        }

        public void Init(int id)
        {
            Load(id);
        }

        public async void Load(int id)
        {
            Pattern pattern = await _client.Get(id);
        }
    }
}