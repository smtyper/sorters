using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Options;
using SortingResearch.Sorters;

namespace SortingResearch
{
    public class Researcher
    {
        private readonly DataGenerator _dataGenerator;
        private readonly ResearcherSettings _settings;
        private readonly IReadOnlyCollection<Sorter> _sorters;

        public Researcher(ShellSorter shellSorter, QuickSorter quickSorter, MergeSorter mergeSorter,
            HeapSorter heapSorter, RadixSorter radixSorter, BuiltInSorter builtInSorter, DataGenerator dataGenerator,
            IOptions<ResearcherSettings> options)
        {
            _dataGenerator = dataGenerator;
            _settings = options.Value;
            _sorters = new Sorter[] { shellSorter, quickSorter, mergeSorter, heapSorter, radixSorter, builtInSorter };
        }

        public void Research()
        {
        }
    }

    public class ResearcherSettings
    {
        [Required]
        public IReadOnlyCollection<TypeCode> Types { get; set; }

        [Required]
        public IReadOnlyCollection<int> CollectionsLength { get; set; }
    }
}
