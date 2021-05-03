using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace SortingResearch
{
    public class Researcher
    {
        private readonly ResearcherSettings _settings;
        private readonly DataGenerator _dataGenerator;

        public Researcher(DataGenerator dataGenerator, IOptions<ResearcherSettings> options)
        {
            _dataGenerator = dataGenerator;
            _settings = options.Value;
        }

        public void Research()
        {
        }
    }

    public class ResearcherSettings
    {
        [Required]
        public IReadOnlyCollection<int> CollectionsLength { get; set; }
    }
}
