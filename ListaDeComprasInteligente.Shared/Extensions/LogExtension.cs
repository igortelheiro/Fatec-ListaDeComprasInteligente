using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;

namespace ListaDeComprasInteligente.Shared.Extensions;

public static class LogExtension
{
    public static void ConfigureSerilog(this IHostBuilder _)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("ApplicationName", $"API Serilog - ListaDeComprasInteligente.API")
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .CreateLogger();
    }


    public static Task LogOnError(this Task task, string message) =>
        task.LogOnError(message);



    public static Task<T> LogOnError<T>(this Task<T> task, string message) where T : class
    {
        try
        {
            return task;
        }
        catch (Exception ex)
        {
            Log.Error(message, ex);
            throw;
        }
    }


    public static Task LogWarningOnError(this Task task, string message) =>
        task.LogWarningOnError(message);


    public static Task<T> LogWarningOnError<T>(this Task<T> task, string message) where T : class
    {
        try
        {
            return task;
        }
        catch (Exception ex)
        {
            Log.Warning(message, ex);
            throw;
        }
    }
}