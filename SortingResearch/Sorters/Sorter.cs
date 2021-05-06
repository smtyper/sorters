using System;
using System.Linq;

namespace SortingResearch.Sorters
{
    public abstract class Sorter
    {
        public T[] GetSortedCopy<T>(T[] sourceArray) where T : IComparable<T>
        {
            var resultArray = sourceArray.ToArray();
            Sort(resultArray);

            return resultArray;
        }

        protected abstract void Sort<T>(T[] array) where T : IComparable<T>;
    }
}
