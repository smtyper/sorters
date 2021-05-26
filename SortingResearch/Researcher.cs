using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.Options;
using SortingResearch.Models;
using SortingResearch.Sorters;

namespace SortingResearch
{
    public class Researcher
    {
        private readonly DataGenerator _dataGenerator;
        private readonly ResearcherSettings _settings;
        private readonly IReadOnlyCollection<Sorter> _sorters;

        public Researcher(BubleSorter bubleSorter, ShellSorter shellSorter, QuickSorter quickSorter,
            MergeSorter mergeSorter,
            HeapSorter heapSorter, RadixSorter radixSorter, BuiltInSorter builtInSorter, DataGenerator dataGenerator,
            IOptions<ResearcherSettings> options)
        {
            _dataGenerator = dataGenerator;
            _settings = options.Value;
            _sorters = new Sorter[]
            {
                bubleSorter, shellSorter, quickSorter, mergeSorter, heapSorter, radixSorter, builtInSorter
            };
        }

        public async Task ResearchAsync()
        {
            var allMeasurements = MeasureSorters();

            foreach (var (generationType, dictionary) in allMeasurements)
            foreach (var (type, measurements) in dictionary)
            {
                var aggregations = measurements.GroupBy(measure => (measure.SorterName, measure.ArrayLength))
                    .Select(group => new Measurement
                    {
                        SorterName = group.Key.SorterName,
                        ArrayGenerationType = group.First().ArrayGenerationType,
                        ArrayType = group.First().ArrayType,
                        ArrayLength = group.First().ArrayLength,
                        Elapsed = TimeSpan.FromTicks((long)group.Average(measurement => measurement.Elapsed.Ticks))
                    })
                    .GroupBy(measurement => measurement.SorterName)
                    .Select(group => new Aggregation
                    {
                        SorterName = group.First().SorterName,
                        ElapsedTimes = group.OrderBy(measurement => measurement.ArrayLength)
                            .ToDictionary(measurement => measurement.ArrayLength,
                                measurement => measurement.Elapsed)
                    })
                    .ToArray();

                await SaveAggregation(aggregations, generationType, type);
            }
        }

        private async Task SaveAggregation(IReadOnlyCollection<Aggregation> aggregations,
            ArrayGenerationType generationType, TypeCode type)
        {
            var filePath = Path.Combine(_settings.DestinationFolderPath, generationType.ToString(), $"{type}.csv");

            if (File.Exists(filePath))
                File.Delete(filePath);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await using var writer = new StreamWriter(filePath);
            await using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csvWriter.WriteField("SorterName");
            foreach (var length in _settings.CollectionsLength.OrderBy(length => length))
                csvWriter.WriteField(length);
            await csvWriter.NextRecordAsync();

            foreach (var aggregation in aggregations)
            {
                csvWriter.WriteField(aggregation.SorterName);
                foreach (var elapsed in aggregation.ElapsedTimes.Values)
                    csvWriter.WriteField(elapsed.TotalMilliseconds);

                await csvWriter.NextRecordAsync();
            }
        }

        private Dictionary<ArrayGenerationType, Dictionary<TypeCode, IReadOnlyCollection<Measurement>>>
            MeasureSorters() => Enum.GetValues<ArrayGenerationType>()
            .ToDictionary(generationType => generationType, generationType => _settings.Types
                .ToDictionary(type => type, type => type switch
                {
                    TypeCode.Byte => GetMeasurements<byte>(generationType),
                    TypeCode.Int32 => GetMeasurements<int>(generationType),
                    TypeCode.String => GetMeasurements<string>(generationType),
                    TypeCode.DateTime => GetMeasurements<DateTime>(generationType),
                    _ => throw new NotImplementedException()
                }));

        private IReadOnlyCollection<Measurement> GetMeasurements<T>(ArrayGenerationType generationType)
            where T : IComparable<T> => _settings.CollectionsLength.SelectMany(length =>
            {
                var array = _dataGenerator.GetGenerationMethod<T>(generationType)(length);
                var measurements = new ConcurrentBag<Measurement>();

                var actions = _sorters.Select(sorter => new Action(() =>
                    {
                        var repeats = sorter.MeasureSorting(array, _settings.Repeats, generationType);
                        foreach (var repeat in repeats)
                            measurements.Add(repeat);
                    }))
                    .ToArray();
                Parallel.Invoke(actions);

                return measurements;
            })
            .ToArray();
    }

    public class ResearcherSettings
    {
        [Range(1, 10)]
        public int Repeats { get; set; }

        [Required]
        public string DestinationFolderPath { get; set; }

        [Required]
        public IReadOnlyCollection<TypeCode> Types { get; set; }

        [Required]
        public IReadOnlyCollection<int> CollectionsLength { get; set; }
    }
}
