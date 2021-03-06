using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace SortingResearch.Sorters
{
    public class RadixSorter : Sorter
    {
        private readonly RadixSorterSettings _settings;

        public RadixSorter(IOptions<RadixSorterSettings> options) => _settings = options.Value;

        protected override T[] Sort<T>(T[] array) => Type.GetTypeCode(typeof(T)) switch
        {
            TypeCode.Byte => RadixSort(array as byte[], 1, (num, _) => num) as T[],
            TypeCode.Int32 => RadixSort(array as int[], _settings.MaxIntegerRank, GetNumberByRank) as T[],
            TypeCode.String => RadixSort(array as string[], _settings.MaxStringRank, GetStringByRank) as T[],
            TypeCode.DateTime => RadixSort(array as DateTime[], 3, GetDateNumberByRank) as T[],
            _ => throw new NotImplementedException()
        };

        private T[] RadixSort<T, TRadixValue>(T[] array, int maxRank, Func<T, int, TRadixValue> getValueByRank)
            where T : IComparable<T>
        {
            var indexСalculations = Enumerable.Range(0, maxRank)
                .ToDictionary(iteration => iteration, iteration =>
                {
                    var currentIndex = 0;
                    var indexAddition = 0;

                    return array.Select(value => getValueByRank(value, iteration))
                        .GroupBy(value => value)
                        .OrderBy(group => group.Key)
                        .ToDictionary(group => group.Key, group =>
                        {
                            currentIndex += indexAddition;
                            indexAddition = group.Count();

                            return currentIndex;
                        });
                });

            var currentArray = array;
            for (var i = 0; i < maxRank; i++)
            {
                var sortingResult = new T[array.Length];
                foreach (var value in currentArray)
                {
                    var number = getValueByRank(value, i);
                    sortingResult[indexСalculations[i][number]] = value;
                    indexСalculations[i][number]++;
                }

                currentArray = sortingResult;
            }

            return currentArray;
        }

        private static int GetNumberByRank(int number, int rank) => number / (int)Math.Pow(10, rank) % 10;

        private string GetStringByRank(string source, int rank) => _settings.MaxStringRank - 1 - rank >= source.Length ?
            string.Empty : source[_settings.MaxStringRank - 1 - rank].ToString();

        private long GetDateNumberByRank(DateTime dateTime, int rank) => rank switch
        {
            0 => dateTime.Day,
            1 => dateTime.Month,
            2 => dateTime.Year,
            _ => throw new NotImplementedException()
        };
    }

    public class RadixSorterSettings
    {
        public int MaxIntegerRank { get; set; }

        public int MaxStringRank { get; set; }
    }
}
