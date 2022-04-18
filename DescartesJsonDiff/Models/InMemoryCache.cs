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

        public static void AddToCache(string key, byte[] value)
        {
            _memoryCache.Set(key, value);
        }

        public static byte[] GetFromCache(string key)
        {
            return _memoryCache.Get(key) as byte[];
        }
    }
}
