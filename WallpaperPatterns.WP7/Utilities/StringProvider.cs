using WallpaperPatterns.WP7.Resources;

namespace WallpaperPatterns.WP7.Utilities
{
    /// <summary>
    /// Localized resource provider
    /// </summary>
    public sealed class StringProvider
    {
        private readonly Strings resources = new Strings();

        public Strings Resources
        {
            get
            {
                return resources;
            }
        }
    }
}
