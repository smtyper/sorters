using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SortingResearch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<DataGenerator>();
                });
    }
}
