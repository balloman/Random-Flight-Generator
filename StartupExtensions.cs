using System.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;

namespace Random_Realistic_Flight;

public static class StartupExtensions
{
    public static void ConfigureForwardedHeaders(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
    }

    public static void ConfigurePathBase(this WebApplication app)
    {
        var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_PATHBASE");
        if (pathBase is null)
        {
            return;
        }

        if (!pathBase.StartsWith("/"))
        {
            pathBase = "/" + pathBase;
        }

        if (pathBase.EndsWith("/"))
        {
            pathBase = pathBase[..^1];
        }
        app.Use((context, next) =>
        {
            context.Request.PathBase = new PathString(pathBase);
            return next(context);
        });
    }
}
