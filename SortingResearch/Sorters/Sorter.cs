using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SortingResearch.Models;

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

        public IReadOnlyCollection<Measurement> MeasureSorting<T>(T[] array, int repeats,
            ArrayGenerationType generationType) where T : IComparable<T> => Enumerable.Range(0, repeats)
                .Select(_ =>
                {
                    var arrayCopy = array.ToArray();
                    var measurement = new Measurement
                    {
                        SorterName = Name,
                        ArrayGenerationType = generationType,
                        ArrayType = Type.GetTypeCode(typeof(T)),
                        ArrayLength = Sort(arrayCopy).Length,
                        Elapsed = Stopwatch.Elapsed
                    };

                    return measurement;
                })
                .ToArray();

        protected abstract T[] Sort<T>(T[] array) where T : IComparable<T>;
    }
}
