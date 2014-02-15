using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using Newtonsoft.Json.Linq;
using WallpaperPatterns.Core.Error;

namespace WallpaperPatterns.Core.Service
{
    public class PatternClient : IPatternClient
    {
        private const string PatternsUrl = "http://www.colourlovers.com/api/patterns&format=json";
        private const string NewestPatternsUrl = "http://www.colourlovers.com/api/patterns/new?format=json";
        private const string TopPatternsUrl = "http://www.colourlovers.com/api/patterns/top?format=json";
        private const string RandomPatternsUrl = "http://www.colourlovers.com/api/patterns/random&?ormat=json";
        private const string SinglePatternUrl = "http://www.colourlovers.com/api/pattern/{0}?format=json";
        private const string SearchPatternUrl = "http://www.colourlovers.com/api/pattern?keywords={0}&format=json";

        public async Task<List<Pattern>> Newest(int offset = 0)
        {
            return await Load(NewestPatternsUrl, offset);
        }

        public async Task<List<Pattern>> Top(int offset = 0)
        {
            return await Load(TopPatternsUrl, offset);
        }

        public async Task<List<Pattern>> Random(int offset = 0)
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

        public async Task<List<Pattern>> Search(string term, int offset)
        {
            string url = string.Format(SearchPatternUrl, term);
            return await Load(url, offset);
        }

        private string BuildUrl(string baseUrl, int offset)
        {
            string url = baseUrl;
            if (offset > 0) url += "&resultOffset=" + offset;
            return url;
        }

        private async Task<List<Pattern>> Load(string baseUrl, int offset)
        {
            try
            {
                var url = BuildUrl(baseUrl, offset);
                using (var handler = new HttpClientHandler())
                {
                    if (handler.SupportsAutomaticDecompression)
                    {
                        handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    }

                    using (var client = new HttpClient(handler))
                    {
                        string result = await client.GetStringAsync(url);
                        JArray items = JArray.Parse(result);
                        List<Pattern> patterns = items.Select(item => item.ToObject<Pattern>()).ToList();
                        return patterns;
                    }
                }
            }
            catch (Exception e)
            {
                Mvx.Resolve<IErrorReporter>().ReportError("Could not load patterns", e);
            }
            return new List<Pattern>();
        }
    }
}