using NLayer.ApiGateway.ConfigurationOptions;
using NLayer.ApiGateway.HttpMessageHandlers;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var configuration = builder.Configuration;

var appSettings = new AppSettings();

configuration.Bind(appSettings);

services.AddOcelot()
    .AddDelegatingHandler<DebuggingHandler>(true);


services.PostConfigure<FileConfiguration>(fileConfiguration =>
{
    foreach (var route in appSettings.Ocelot.Routes.Select(x => x.Value))
    {
        var uri = new Uri(route.Downstream);

        foreach (var pathTemplate in route.UpstreamPathTemplates)
        {
            fileConfiguration.Routes.Add(new FileRoute
            {
                UpstreamPathTemplate = pathTemplate,
                DownstreamPathTemplate = pathTemplate,
                DownstreamScheme = uri.Scheme,
                DownstreamHostAndPorts = new List<FileHostAndPort>
                {
                    new FileHostAndPort{ Host = uri.Host, Port = uri.Port }
                }
            });
        }
    }

    foreach (var route in fileConfiguration.Routes)
    {
        if (string.IsNullOrWhiteSpace(route.DownstreamScheme))
        {
            route.DownstreamScheme = appSettings?.Ocelot?.DefaultDownstreamScheme;
        }

        if (string.IsNullOrWhiteSpace(route.DownstreamPathTemplate))
        {
            route.DownstreamPathTemplate = route.UpstreamPathTemplate;
        }
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseWebSockets();
await app.UseOcelot();

app.Run();

