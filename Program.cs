using Serilog;

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console()
                    .ReadFrom.Configuration(ctx.Configuration));
    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.MapGet("/", () => "Hello World!");
    app.MapGet("/greetings/{name}", (string name)=> $"Hello {name} ");
    app.MapGet("/oops",new Func<string>(() => throw new InvalidOperationException("Oops!")));

    app.Run();
    
}
catch (Exception ex)
{
     // TODO
     Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
