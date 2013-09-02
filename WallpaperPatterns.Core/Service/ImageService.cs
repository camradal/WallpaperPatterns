using System.Collections.Generic;
using Cirrious.MvvmCross.Plugins.File;

namespace WallpaperPatterns.Core.Service
{
    public interface IImageService
    {
        void Save(string name, IEnumerable<byte> bytes);
        byte[] Load(string name);
    }

    public class ImageService : IImageService
    {
        private readonly IMvxFileStore _store;

        public ImageService(IMvxFileStore store)
        {
            this._store = store;
        }

        public void Save(string name, IEnumerable<byte> bytes)
        {
            _store.WriteFile(name, bytes);
        }

        public byte[] Load(string name)
        {
            byte[] bytes;
            return _store.TryReadBinaryFile(name, out bytes) ? bytes : null;
        }
    }
}