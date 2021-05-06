using System;
using System.Linq;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace SortingResearch
{
    public class DataGenerator
    {
        private readonly IRandomizerNumber<byte> _randomizerByte;
        private readonly IRandomizerDateTime _randomizerDateTime;
        private readonly IRandomizerNumber<int> _randomizerInteger;
        private readonly IRandomizerString _randomizerString;

        public DataGenerator()
        {
            _randomizerByte = RandomizerFactory.GetRandomizer(new FieldOptionsByte { Min = 0, Max = 9 });
            _randomizerInteger = RandomizerFactory.GetRandomizer(new FieldOptionsInteger
            {
                Min = int.MinValue, Max = int.MaxValue
            });
            _randomizerString = RandomizerFactory.GetRandomizer(new FieldOptionsText
            {
                UseSpecial = false, Min = 8, Max = 16
            });
            _randomizerDateTime = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime { IncludeTime = true });
        }

        public T[] GetArray<T>(int count) => Enumerable.Range(0, count)
            .Select(_ => (T)(Type.GetTypeCode(typeof(T)) switch
            {
                TypeCode.Byte => _randomizerByte.Generate() as object,
                TypeCode.Int32 => _randomizerInteger.Generate() as object,
                TypeCode.String => _randomizerString.Generate() as object,
                TypeCode.DateTime => _randomizerDateTime.Generate() as object,
                _ => throw new NotImplementedException()
            }))
            .ToArray();
    }
}
