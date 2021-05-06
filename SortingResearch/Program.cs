using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SortingResearch.Sorters;

namespace SortingResearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var researcher = host.Services.GetService<Researcher>();
            researcher.Research();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<DataGenerator>();

                    services.AddSingleton<ShellSorter>();
                    services.AddSingleton<QuickSorter>();
                    services.AddSingleton<MergeSorter>();

                    services.AddSingleton<Researcher>().AddOptions<ResearcherSettings>()
                        .Bind(hostContext.Configuration.GetSection("Researcher"));
                });
    }
}
