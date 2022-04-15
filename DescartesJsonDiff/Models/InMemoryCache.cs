using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Models
{
    public static class InMemoryCache
    {
        private static readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        public static void AddToCache(string key, string value)
        {
            _memoryCache.Set(key, value);
        }

        public static string GetFromCache(string key)
        {
            return _memoryCache.Get(key) as string;
        }

        public static void RemoveFromCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
