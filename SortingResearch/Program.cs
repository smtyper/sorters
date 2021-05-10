using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SortingResearch;
using SortingResearch.Sorters;

await CreateHostBuilder(args).Build().Services.GetService<Researcher>().ResearchAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<DataGenerator>().AddOptions<DataGeneratorSettings>()
                .Bind(hostContext.Configuration.GetSection("DataGenerator"))
                .ValidateDataAnnotations();

            services.AddSingleton<BubleSorter>();
            services.AddSingleton<ShellSorter>();
            services.AddSingleton<QuickSorter>();
            services.AddSingleton<MergeSorter>();
            services.AddSingleton<HeapSorter>();
            services.AddSingleton<RadixSorter>().AddOptions<RadixSorterSettings>()
                .Configure(options =>
                {
                    var dataGeneratorSection = hostContext.Configuration.GetSection("DataGenerator");

                    options.MaxIntegerRank = dataGeneratorSection["IntegerMax"].Length;
                    options.MaxStringRank = int.Parse(dataGeneratorSection["StringMaxLength"]);
                });
            services.AddSingleton<BuiltInSorter>();

            services.AddSingleton<Researcher>().AddOptions<ResearcherSettings>()
                .Bind(hostContext.Configuration.GetSection("Researcher"))
                .ValidateDataAnnotations();
        });
