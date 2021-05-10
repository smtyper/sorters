using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SortingResearch.Sorters;

namespace SortingResearch
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var researcher = host.Services.GetService<Researcher>();
            await researcher.ResearchAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<DataGenerator>().AddOptions<DataGeneratorSettings>()
                        .Bind(hostContext.Configuration.GetSection("DataGenerator"))
                        .ValidateDataAnnotations();

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
    }
}
