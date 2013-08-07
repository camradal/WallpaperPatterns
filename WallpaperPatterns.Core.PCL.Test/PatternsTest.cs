using Microsoft.VisualStudio.TestTools.UnitTesting;
using WallpaperPatterns.Core.PCL;

namespace WallpaperPatterns.PCL.Test
{
    [TestClass]
    public class PatternsTest
    {
        [TestMethod]
        public void PatternsGetNewestTest()
        {
            var patterns = new PatternClient();
            var result = patterns.GetNewest().Result;
            Assert.IsTrue(result.Count > 0);
        }
    }
}
