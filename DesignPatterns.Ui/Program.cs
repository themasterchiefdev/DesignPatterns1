using System;
using System.Net.Http;
using System.Threading.Tasks;
using DesignPatterns.Decorator.Interfaces;
using DesignPatterns.Decorator.Services.Repository.UserRepository;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
namespace DesignPatterns.Ui
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMemoryCache();
            builder.Services.AddScoped<IUserRepository, UserRepositoryCachingServiceDecorator>();
            builder.Services.AddScoped(serviceprovider =>
            {
                var httpClient = new HttpClient();
                var memoryCache = serviceprovider.GetService<IMemoryCache>();

                IUserRepository concreteUserRepository = new UserRepository(httpClient);
                IUserRepository withCachingDecorator = new UserRepositoryCachingServiceDecorator(concreteUserRepository, memoryCache);
                return withCachingDecorator;
            });

            await builder.Build().RunAsync();
        }
    }
}
