namespace SortingResearch.Sorters
{
    public class ShellSorter : Sorter
    {
        protected override void Sort<T>(T[] array)
        {
            var step = array.Length / 2;

            while (step > 0)
            {
                for (var i = 0; i + step < array.Length; i++)
                {
                    var j = i + step;
                    var spanLastValue = array[j];

                    while (j - step >= 0 && spanLastValue.CompareTo(array[j - 1]) < 0)
                    {
                        array[j] = array[j - step];
                        j -= step;
                    }

                    array[j] = spanLastValue;
                }

                step /= 2;
            }
        }
    }
}
