using System;

namespace SortingResearch.Sorters
{
    public class QuickSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            Stopwatch.Restart();

            QuickSort(array, 0, array.Length - 1);

            Stopwatch.Stop();

            return array;
        }

        private static void QuickSort<T>(T[] array, int low, int high) where T : IComparable<T>
        {
            if (low < high)
            {
                var partitionIndex = Partition(array, low, high);

                QuickSort(array, low, partitionIndex - 1);
                QuickSort(array, partitionIndex + 1, high);
            }
        }

        private static int Partition<T>(T[] array, int low, int high) where T : IComparable<T>
        {
            void Swap(int first, int second)
            {
                var firstValue = array[first];

                array[first] = array[second];
                array[second] = firstValue;
            }

            var pivot = array[high];

            var lowIndex = low - 1;

            for (var j = low; j < high; j++)
                if (array[j].CompareTo(pivot) <= 0)
                {
                    lowIndex++;
                    Swap(lowIndex, j);
                }

            Swap(lowIndex + 1, high);

            return lowIndex + 1;
        }

    }
}
