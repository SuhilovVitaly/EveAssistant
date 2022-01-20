using Microsoft.Extensions.Caching.Memory;
using System;
using System.Drawing;
using System.IO;

namespace EveAssistant.Graphic
{
    public class ImageFactory
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public Bitmap Load(string fileName)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName.Replace(@"/",@"\"));

            var image = GetOrCreate(path, LoadBitmap);

            return image;
        }

        private static Bitmap LoadBitmap(string fileName)
        {
            return (Bitmap)Image.FromFile(fileName);
        }

        public Bitmap GetOrCreate(string key, Func<string, Bitmap> createItem)
        {
            if (!_cache.TryGetValue(key, out Bitmap cacheEntry))// Look for cache key.
            {
                // Key not in cache, so get data.
                cacheEntry = createItem(key);

                // Save data in cache.
                _cache.Set(key, cacheEntry);
            }
            return cacheEntry;
        }
    }
}
