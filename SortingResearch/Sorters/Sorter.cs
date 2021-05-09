using System;
using System.Diagnostics;
using System.Linq;

namespace SortingResearch.Sorters
{
    public abstract class Sorter
    {
        private readonly string _sorterName;
        protected readonly Stopwatch Stopwatch;

        protected Sorter()
        {
            Stopwatch = new Stopwatch();
            _sorterName = this.GetType().Name;
        }

        public string Name => _sorterName;


        public (T[] Result, TimeSpan Measurements) MeasureSorting<T>(T[] sourceArray) where T : IComparable<T> =>
            (Sort(sourceArray.ToArray()), Stopwatch.Elapsed);

        protected abstract T[] Sort<T>(T[] array) where T : IComparable<T>;
    }
}
