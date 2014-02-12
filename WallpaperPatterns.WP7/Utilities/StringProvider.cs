using WallpaperPatterns.WP7.Resources;

namespace WallpaperPatterns.WP7.Utilities
{
    /// <summary>
    /// Localized resource provider
    /// </summary>
    public sealed class StringProvider
    {
        private readonly AppResources resources = new AppResources();

        public AppResources Resources
        {
            get
            {
                return resources;
            }
        }
    }
}
