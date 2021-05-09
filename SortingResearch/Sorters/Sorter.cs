using System;
using System.Diagnostics;
using System.Linq;

namespace SortingResearch.Sorters
{
    public abstract class Sorter
    {
        protected readonly Stopwatch Stopwatch;

        protected Sorter()
        {
            Stopwatch = new Stopwatch();
            Name = GetType().Name;
        }

        public string Name { get; }


        public (T[] Result, TimeSpan Measurements) MeasureSorting<T>(T[] sourceArray) where T : IComparable<T> =>
            (Sort(sourceArray.ToArray()), Stopwatch.Elapsed);

        protected abstract T[] Sort<T>(T[] array) where T : IComparable<T>;
    }
}
