using DescartesJsonDiff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Services.Interfaces
{
    public interface IDifferentialJsonService
    {
        public void CreateJsonData(string id, JsonInput data, string side);
        public JsonResponse GetJsonDiff(string id);
    }
}
