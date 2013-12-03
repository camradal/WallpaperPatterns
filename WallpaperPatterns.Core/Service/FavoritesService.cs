using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Plugins.File;
using Cirrious.MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WallpaperPatterns.Core.Messages;

namespace WallpaperPatterns.Core.Service
{
    public class FavoritesService : IFavoritesService
    {
        private const string FavoritesFile = "favorites.json";

        private readonly IMvxFileStore _fileStore;
        private readonly IMvxMessenger _messenger;
        private readonly object _locker = new object();

        private List<Pattern> _favorites = new List<Pattern>();

        public FavoritesService(IMvxFileStore fileStore, IMvxMessenger messenger)
        {
            _fileStore = fileStore;
            _messenger = messenger;

            string contents;
            if (_fileStore.TryReadTextFile(FavoritesFile, out contents))
            {
                Load(contents);
            }
            else
            {
                Save();
            }
        }

        public void Insert(Pattern pattern)
        {
            Pattern toInsert = _favorites.FirstOrDefault(p => p.Id == pattern.Id);
            if (toInsert == null)
            {
                _favorites.Add(pattern);
                _messenger.Publish(new FavoritesChangedMessage(this));
                Save();
            }
        }

        public void Delete(Pattern pattern)
        {
            Pattern toDelete = _favorites.FirstOrDefault(p => p.Id == pattern.Id);
            if (toDelete != null)
            {
                _favorites.Remove(toDelete);
                _messenger.Publish(new FavoritesChangedMessage(this));
                Save();
            }
        }

        public bool Contains(Pattern pattern)
        {
            return _favorites.Any(p => p.Id == pattern.Id);
        }

        public List<Pattern> All()
        {
            return new List<Pattern>(_favorites);
        }

        private void Load(string contents)
        {
            lock (_locker)
            {
                JArray items = JArray.Parse(contents);
                _favorites = items.Select(item => item.ToObject<Pattern>()).ToList();
            }
        }

        private void Save()
        {
            lock (_locker)
            {
                string serialized = JsonConvert.SerializeObject(_favorites);
                // for some reason need to delete, revisit this sometime
                if (_fileStore.Exists(FavoritesFile))
                {
                    _fileStore.DeleteFile(FavoritesFile);
                }
                _fileStore.WriteFile(FavoritesFile, serialized);
            }
        }
    }
}