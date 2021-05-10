using System;
using System.Collections.Generic;
using System.Linq;
using CsvHelper.Configuration;

namespace SortingResearch.Models
{
    public class Aggregation
    {
        public string SorterName { get; init; }

        public Dictionary<int, TimeSpan> ElapsedTimes { get; init; }
    }
}
