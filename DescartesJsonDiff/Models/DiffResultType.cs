using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Models
{
    public enum DiffResultType
    {
        ContentDoNotMatch,
        Equals,
        SizeDoNotMatch
    }
}
