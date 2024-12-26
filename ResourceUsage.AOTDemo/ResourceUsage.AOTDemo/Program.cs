using Microsoft.AspNetCore.Hosting;
using ResourceUsage.AOTDemo;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/";
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});
var app = builder.Build();

var benchApi = app.MapGroup("/benchmark");
benchApi.MapGet("/run", handler: (double limit) =>
{

    using (Process process = Process.GetCurrentProcess())
    {
        // Inicia a coleta de métricas
        process.Refresh();
        // Memória
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var primes = PrimeCalculator.CalculatePrimes(limit);

        stopwatch.Stop();

        // CPU e tempo de processamento
        TimeSpan cpuUsed = process.TotalProcessorTime;
        int coreCount = Environment.ProcessorCount; // Número de núcleos

        // Coletando dados de uso de memória
        long memoryUsed = Process.GetCurrentProcess().WorkingSet64;
        //Console.WriteLine($"Memory used: {memoryUsed / 1024 / 1024} MB");
        Console.WriteLine($"Memory Used: {memoryUsed / 1024 / 1024} MB");

        return Results.Ok(new BenchmarkResult(
            "Prime numbers",
            limit,
            primes.Count,
            cpuUsed.TotalMilliseconds,
            coreCount,
            memoryUsed / 1024 / 1024,
            stopwatch.ElapsedMilliseconds,
            primes
        ));

    }

});

// app.MapGet("/index", async context =>
// {
// context.Response.ContentType = "text/html";
// await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "index.html"));
// v});



app.UseStaticFiles(); // Mapeia Razor Pages no aplicativo
app.MapRazorPages(); // Mapeia Razor Pages no aplicativo
app.Run();




public record BenchmarkResult(string TestType, double InputLimit, double PrimesCount, double ProcessorTime, double ProcessorCount, double Memory, double Duration, List<double> Primes);
[JsonSerializable(typeof(BenchmarkResult))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}