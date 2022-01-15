using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

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
        app.Use((context, next) =>
        {
            context.Request.PathBase = new PathString(pathBase);
            return next(context);
        });
    }

    public static void ConfigureDataProtection(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            return;
        }
        var path = Path.Combine(Directory.GetCurrentDirectory(), "DataProtection");
        builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(path));
    }

    public static void ConfigureCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors();
    }
}
