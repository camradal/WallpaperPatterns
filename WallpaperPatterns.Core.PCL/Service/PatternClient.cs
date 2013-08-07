using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WallpaperPatterns.Core.PCL
{
    public class PatternClient
    {
        private const string PatternsUrl = "http://www.colourlovers.com/api/patterns&format=json";
        private const string NewestPatternsUrl = "http://www.colourlovers.com/api/patterns/new&format=json";
        private const string TopPatternsUrl = "http://www.colourlovers.com/api/patterns/top&format=json";
        private const string RandomPatternsUrl = "http://www.colourlovers.com/api/patterns/random&format=json";
        private const string SinglePatternUrl = "http://www.colourlovers.com/api/pattern/{0}&format=json";

        public async Task<List<Pattern>> GetNewest(int offset = 0)
        {
            return await Load(NewestPatternsUrl, offset);
        }

        public async Task<List<Pattern>> GetTop(int offset = 0)
        {
            return await Load(TopPatternsUrl, offset);
        }

        public async Task<List<Pattern>> GetRandom(int offset = 0)
        {
            return await Load(RandomPatternsUrl, offset);
        }

        public async Task<Pattern> Get(int id)
        {
            string url = string.Format(SinglePatternUrl, id);
            List<Pattern> patterns = await Load(url, 0);
            Pattern pattern = patterns.FirstOrDefault();
            return pattern;
        }

        private string BuildUrl(string baseUrl, int offset)
        {
            string url = baseUrl;
            if (offset > 0) url += "&resultOffset=" + offset;
            return url;
        }

        private async Task<List<Pattern>> Load(string baseUrl, int offset)
        {
            var url = BuildUrl(baseUrl, 0);
            var client = new HttpClient();
            string result = await client.GetStringAsync(url);
            JArray items = JArray.Parse(result);
            List<Pattern> patterns = items.Select(item => item.ToObject<Pattern>()).ToList();
            return patterns;
        }
    }
}