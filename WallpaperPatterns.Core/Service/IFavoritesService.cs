using System.Collections.Generic;

namespace WallpaperPatterns.Core.Service
{
    public interface IFavoritesService
    {
        void Insert(Pattern pattern);
        void Delete(Pattern pattern);
        List<Pattern> All();
    }
}