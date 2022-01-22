using Random_Realistic_Flight;
using Random_Realistic_Flight.Services;
using Random_Realistic_Flight.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureCors();
builder.Services.AddRazorPages().AddSessionStateTempDataProvider();
builder.ConfigureForwardedHeaders();
builder.Services.AddSingleton<IFlightService, AeroDataBoxService>();
builder.Services.AddSingleton<IKeyService, PropertyKeySetter>();
builder.Services.AddSession();

var app = builder.Build();

app.UseCors(options => options.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseForwardedHeaders();
    app.ConfigurePathBase();
}

app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
