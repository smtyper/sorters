namespace SortingResearch.Sorters
{
    public class BubleSorter : Sorter
    {
        protected override T[] Sort<T>(T[] array)
        {
            for (var i = 0; i <= array.Length - 2; i++)
                for (var j = 0; j <= array.Length - 2; j++)
                    if (array[j].CompareTo(array[j + 1]) > 0)
                        Swap(array, j, j + 1);

            return array;
        }
    }
}
