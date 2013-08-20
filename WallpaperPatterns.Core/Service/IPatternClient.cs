using System.Collections.Generic;
using System.Threading.Tasks;

namespace WallpaperPatterns.Core.Service
{
    public interface IPatternClient
    {
        Task<List<Pattern>> Newest(int offset = 0);
        Task<List<Pattern>> Top(int offset = 0);
        Task<List<Pattern>> Random(int offset = 0);
        Task<Pattern> Get(int id);
    }
}