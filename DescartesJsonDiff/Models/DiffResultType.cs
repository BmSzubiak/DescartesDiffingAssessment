using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DescartesJsonDiff.Models
{
    public static class DiffResultType
    {
        public const string ContentDoNotMatch = "ContentDoNotMatch";
        public new const string Equals = "Equals";
        public const string SizeDoNotMatch = "SizeDoNotMatch";

    }
}
