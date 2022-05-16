using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Pokemon.Application.Contracts;
using Pokemon.Infrastructure;

namespace Pokemon.IntegrationTests;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup: class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddHttpClient("PokeApi", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.GetSetting("PokeApiUrl"));
            });
            
            services.AddHttpClient("ShakespeareTranslatorApi", httpClient =>
            {
                httpClient.BaseAddress = new Uri(builder.GetSetting("PokeApiUrl"));
            });

            services.AddScoped<IPokeApiClientService, PokeApiClientService>();

        });
    }
}