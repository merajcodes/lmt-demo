using lmt;
using lmt.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OpenTelemetry.Trace;
using SeliseBlocks.LMT.Client;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(MongoDbSettings.SectionName));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IItemsService, ItemsService>();

builder.Services.AddLmtClient(builder.Configuration);

var lmtOptions = builder.Configuration.GetSection("Lmt").Get<LmtOptions>() ?? new LmtOptions();
var lmtActivitySourceName = lmtOptions.ServiceId;

builder.Services.AddSingleton(new ActivitySource(lmtActivitySourceName));
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerBuilder =>
    {
        tracerBuilder
            .AddSource(lmtActivitySourceName)
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddLmtTracing(lmtOptions);
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
