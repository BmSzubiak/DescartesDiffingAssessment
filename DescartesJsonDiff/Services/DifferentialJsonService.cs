using DescartesJsonDiff.Models;
using DescartesJsonDiff.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Services
{
    public class DifferentialJsonService : IDifferentialJsonService
    {
        public void CreateJsonData(string id, JsonInput input, string side)
        {
            var entryId = id + "-" + side;

            var inputData = Convert.ToBase64String(input.data);
            InMemoryCache.AddToCache(entryId, inputData);
        }

        public JsonResponse GetJsonDiff(string id)
        {
            //Get from cache,
            var leftResult = InMemoryCache.GetFromCache(id + "-" + "left");
            var rightResult = InMemoryCache.GetFromCache(id + "-" + "right");

            return new JsonResponse() 
            {
                DiffResultType = DiffResultType.ContentDoNotMatch,
                Diffs = new List<Differential>()
            };
        }
    }
}
