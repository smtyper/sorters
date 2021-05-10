using System;
using System.Collections.Generic;

namespace SortingResearch.Models
{
    public class Aggregation
    {
        public string SorterName { get; init; }

        public Dictionary<int, TimeSpan> ElapsedTimes { get; init; }
    }
}
