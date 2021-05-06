using System;

namespace SortingResearch.Sorters
{
    public class HeapSorter : Sorter
    {
        protected override void Sort<T>(T[] array)
        {
            void Swap(int first, int second)
            {
                var firstValue = array[first];

                array[first] = array[second];
                array[second] = firstValue;
            }

            for (var i = (array.Length / 2) - 1; i >= 0; i--)
                Heapify(array, array.Length, i);

            for (var i = array.Length - 1; i >= 0; i--)
            {
                Swap(0, i);
                Heapify(array, i, 0);
            }
        }

        private static void Heapify<T>(T[] array, int border, int i) where T : IComparable<T>
        {
            void Swap(int first, int second)
            {
                var firstValue = array[first];

                array[first] = array[second];
                array[second] = firstValue;
            }

            while (true)
            {
                var max = i;
                var left = (2 * i) + 1;
                var right = (2 * i) + 2;

                max = left < border && array[left].CompareTo(array[max]) > 0 ? left : max;
                max = right < border && array[right].CompareTo(array[max]) > 0 ? right : max;

                if (max != i)
                {
                    Swap(i, max);
                    i = max;
                    continue;
                }

                break;
            }
        }
    }
}
