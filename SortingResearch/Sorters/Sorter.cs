using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SortingResearch.Models;

namespace SortingResearch.Sorters
{
    public abstract class Sorter
    {
        protected Sorter() => Name = GetType().Name;

        public string Name { get; }

        public IReadOnlyCollection<Measurement> MeasureSorting<T>(T[] array, int repeats,
            ArrayGenerationType generationType) where T : IComparable<T>
        {
            var measurements = new ConcurrentBag<Measurement>();

            var actions = Enumerable.Range(0, repeats).Select(_ => new Action(() =>
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var sortedArray = Sort(array.ToArray());

                    stopwatch.Stop();

                    var measurement = new Measurement
                    {
                        SorterName = Name,
                        ArrayGenerationType = generationType,
                        ArrayType = Type.GetTypeCode(typeof(T)),
                        ArrayLength = sortedArray.Length,
                        Elapsed = stopwatch.Elapsed
                    };

                    measurements.Add(measurement);
                }))
                .ToArray();

            Parallel.Invoke(actions);

            return measurements;
        }

        protected static void Swap<T>(T[] array, int first, int second)
        {
            var firstValue = array[first];

            array[first] = array[second];
            array[second] = firstValue;
        }

        protected abstract T[] Sort<T>(T[] array) where T : IComparable<T>;
    }
}
