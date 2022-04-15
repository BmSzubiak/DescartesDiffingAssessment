using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Models
{
    public class JsonResponse
    {
        public DiffResultType DiffResultType { get; set; }
        public List<Differential> Diffs { get; set; }
    }
}
