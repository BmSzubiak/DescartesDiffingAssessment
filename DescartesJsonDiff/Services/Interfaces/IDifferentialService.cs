using DescartesJsonDiff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Services.Interfaces
{
    public interface IDifferentialService
    {
        public void CreateJsonData(string id, ApiInput data, string side);
        public ApiResponse GetJsonDiff(string id);
    }
}
