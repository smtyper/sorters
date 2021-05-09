using System;

namespace SortingResearch.Sorters
{
    public class MergeSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            Stopwatch.Restart();

            MergeSort(array, 0, array.Length - 1);

            Stopwatch.Stop();

            return array;
        }

        private static void MergeSort<T>(T[] array, int start, int end) where T : IComparable<T>
        {
            if (start < end)
            {
                var midle = (start + end) / 2;
                MergeSort(array, start, midle);
                MergeSort(array, midle + 1, end);

                Merge(array, start, midle, end);
            }
        }

        private static void Merge<T>(T[] array, int start, int mid, int end) where T : IComparable<T>
        {
            var firstPart = start;
            var secondPart = mid + 1;

            var temporaryArray = new T[end - start + 1];
            var k = 0;

            for (var i = start; i <= end; i++, k++)
                temporaryArray[k] = firstPart > mid ?
                    array[secondPart++] : secondPart > end || array[firstPart].CompareTo(array[secondPart]) < 0 ?
                        array[firstPart++] : array[secondPart++];

            for (var j = 0; j < k; j++) array[start++] = temporaryArray[j];
        }
    }
}
