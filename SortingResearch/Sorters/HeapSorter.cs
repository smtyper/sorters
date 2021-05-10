using System;

namespace SortingResearch.Sorters
{
    public class HeapSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            for (var i = (array.Length / 2) - 1; i >= 0; i--)
                Heapify(array, array.Length, i);

            for (var i = array.Length - 1; i >= 0; i--)
            {
                Swap(array, 0, i);
                Heapify(array, i, 0);
            }

            return array;
        }

        private static void Heapify<T>(T[] array, int border, int i) where T : IComparable<T>
        {
            while (true)
            {
                var max = i;
                var left = (2 * i) + 1;
                var right = (2 * i) + 2;

                max = left < border && array[left].CompareTo(array[max]) > 0 ? left : max;
                max = right < border && array[right].CompareTo(array[max]) > 0 ? right : max;

                if (max != i)
                {
                    Swap(array, i, max);
                    i = max;
                    continue;
                }

                break;
            }
        }
    }
}
