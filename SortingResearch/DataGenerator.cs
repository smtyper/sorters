using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Options;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using SortingResearch.Models;

namespace SortingResearch
{
    public class DataGenerator
    {
        private readonly DataGeneratorSettings _settings;
        private readonly IRandomizerNumber<byte> _randomizerByte;
        private readonly IRandomizerDateTime _randomizerDateTime;
        private readonly IRandomizerNumber<int> _randomizerInteger;
        private readonly IRandomizerString _randomizerString;

        public DataGenerator(IOptions<DataGeneratorSettings> options)
        {
            _settings = options.Value;

            _randomizerByte = RandomizerFactory.GetRandomizer(new FieldOptionsByte
            {
                Min = 0,
                Max = 9
            });
            _randomizerInteger = RandomizerFactory.GetRandomizer(new FieldOptionsInteger
            {
                Min = _settings.IntegerMin,
                Max = _settings.IntegerMax
            });
            _randomizerString = RandomizerFactory.GetRandomizer(new FieldOptionsText
            {
                Min = _settings.StringMinLength,
                Max = _settings.StringMaxLength,
                UseSpecial = false,
            });
            _randomizerDateTime = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime
            {
                From = _settings.DateTimeMin,
                To = _settings.DateTimeMax,
                IncludeTime = false
            });
        }

        public Func<int, T[]> GetGenerationMethod<T>(ArrayGenerationType type) => type switch
        {
            ArrayGenerationType.Random => GetArray<T>,
            ArrayGenerationType.DescendingSorted => GetDescendingSortedArray<T>,
            ArrayGenerationType.PartiallySorted => GetPartiallySortedArray<T>,
            _ => throw new NotImplementedException()
        };

        private T[] GetArray<T>(int count) => GetEnumerable<T>(count).ToArray();

        private T[] GetPartiallySortedArray<T>(int count)
        {
            var array = GetArray<T>(count);
            var sortedCount = Convert.ToInt32(_settings.PartialSortPercent * count);

            var result = array.Take(sortedCount)
                .OrderBy(value => value)
                .Concat(array.TakeLast(count - sortedCount))
                .ToArray();

            return result;
        }

        private T[] GetDescendingSortedArray<T>(int count) => GetEnumerable<T>(count)
            .OrderByDescending(value => value)
            .ToArray();

        private IEnumerable<T> GetEnumerable<T>(int count) => Enumerable.Range(0, count)
            .Select(_ => (T)(Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte => _randomizerByte.Generate() as object,
                TypeCode.Int32 => _randomizerInteger.Generate() as object,
                TypeCode.String => _randomizerString.Generate() as object,
                TypeCode.DateTime => _randomizerDateTime.Generate() as object,
                _ => throw new NotImplementedException()
            }));
    }

    public class DataGeneratorSettings
    {
        public int IntegerMin { get; set; }

        public int IntegerMax { get; set; }

        public int StringMinLength { get; set; }

        public int StringMaxLength { get; set; }

        public DateTime DateTimeMin { get; set; }

        public DateTime DateTimeMax { get; set; }

        [Range(0, 1)]
        public double PartialSortPercent { get; set; }
    }
}
