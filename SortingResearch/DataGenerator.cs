using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;

namespace SortingResearch
{
    public class DataGenerator
    {
        private readonly IRandomizerBytes _randomizerBytes;
        private readonly IRandomizerDateTime _randomizerDateTime;
        private readonly IRandomizerGuid _randomizerGuid;
        private readonly IRandomizerString _randomizerString;

        public DataGenerator()
        {
            _randomizerString = RandomizerFactory.GetRandomizer(new FieldOptionsText
            {
                UseSpecial = false,
                Min = 4,
                Max = 16
            });
            _randomizerBytes = RandomizerFactory.GetRandomizer(new FieldOptionsBytes { Min = 0, Max = 9 });
            _randomizerDateTime = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime { IncludeTime = true });
            _randomizerGuid = RandomizerFactory.GetRandomizer(new FieldOptionsGuid());
        }
    }
}
