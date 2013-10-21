using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WallpaperPatterns.Core.Service;

namespace WallpaperPatterns.Store81
{
    public class IncrementalLoadingTop : IncrementalLoadingBase
    {
        private readonly IPatternClient _client;

        public IncrementalLoadingTop(IPatternClient client)
        {
            _client = client;
        }

        protected override Task<List<Pattern>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            var loadMoreItemsOverrideAsync = _client.Top(this.Count);
            return loadMoreItemsOverrideAsync;
        }

        protected override bool HasMoreItemsOverride()
        {
            return true;
        }
    }
}