using System;

namespace SortingResearch.Sorters
{
    public class BuiltInSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            Array.Sort(array);

            return array;
        }
    }
}
