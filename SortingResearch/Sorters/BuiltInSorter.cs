using System;

namespace SortingResearch.Sorters
{
    public class BuiltInSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            Stopwatch.Restart();

            Array.Sort(array);

            Stopwatch.Stop();

            return array;
        }
    }
}
