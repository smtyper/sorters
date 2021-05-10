using System;

namespace SortingResearch.Models
{
    public class Measurement
    {
        public string SorterName { get; init; }

        public ArrayGenerationType ArrayGenerationType { get; init; }

        public TypeCode ArrayType { get; init; }

        public int ArrayLength { get; init; }

        public TimeSpan Elapsed { get; init; }
    }
}
