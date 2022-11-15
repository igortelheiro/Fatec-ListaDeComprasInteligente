using ListaDeComprasInteligente.Scraper;
using ListaDeComprasInteligente.Scraper.Interfaces;
using ListaDeComprasInteligente.Service;
using ListaDeComprasInteligente.Service.Interfaces;
using ListaDeComprasInteligente.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog(Log.Logger).ConfigureSerilog();
    builder.Services.AddHttpClient();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSwaggerGenNewtonsoftSupport();
    builder.Services.AddSingleton<IListaComprasBuilderService, ListaComprasBuilderService>();
    builder.Services.AddSingleton<IScraperService, HttpScraperService>();
    builder.Services.AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });

try
{
    var app = builder.Build();
        app.UseCors(config =>
        {
            config.AllowAnyHeader();
            config.AllowAnyMethod();
            config.AllowAnyOrigin();
        });
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
}
catch (Exception ex)
{
    Log.Error("O programa foi encerrado inesperadamente {ex}", ex);
    Log.CloseAndFlush();
}