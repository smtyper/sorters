using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

                    services.AddSingleton<Researcher>().AddOptions<ResearcherSettings>()
                        .Bind(hostContext.Configuration.GetSection("Researcher"));
                });
    }
}
