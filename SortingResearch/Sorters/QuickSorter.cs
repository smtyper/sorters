using System;

namespace SortingResearch.Sorters
{
    public class QuickSorter : Sorter
    {
        protected override void Sort<T>(T[] array) => QuickSort(array, 0, array.Length - 1);

        private static void QuickSort<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            if (start < end)
            {
                var pivotPosition = Partition(array, start, end);
                QuickSort(array, start, pivotPosition - 1);
                QuickSort(array, pivotPosition + 1, end);
            }
        }

        private static int Partition<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            void Swap(int first, int second)
            {
                var firstValue = array[first];

                array[first] = array[second];
                array[second] = firstValue;
            }

            var i = start - 1;
            var pivot = array[end];
            for (var j = start; j <= end - 1; j++)
                if (array[j].CompareTo(pivot) <= 0)
                {
                    i++;
                    Swap(i, j);
                }

            Swap(end, i + 1);

            return i + 1;
        }
    }
}
