using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SynDataService.Http;
using PlatformService.AsyncDataServices;
using PlatformService.SyncDataService.Grpc;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using MSSQL DB");

    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
    );
}
else
{
    builder.WebHost
    .ConfigureKestrel(serverOptions =>
    {
        serverOptions.ListenLocalhost(
            int.Parse(builder.Configuration["GrpcPort"]),
            opt =>
            {
                opt.Protocols = HttpProtocols.Http2;
            }
        );

        serverOptions.ListenLocalhost(
            int.Parse(builder.Configuration["WebApiPort"]),
            opt =>
            {
                opt.Protocols = HttpProtocols.Http2;
            }
        );
    });

    Console.WriteLine("--> Using InMem DB");

    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseInMemoryDatabase("InMem")
    );
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddGrpc();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcPlatformService>();

app.MapGet("/protos/platform.proto", async context =>
{
    await context.Response.WriteAsync(await File.ReadAllTextAsync("Protos/platform.proto"));
});

app.PrePopulation(app.Environment.IsProduction());

app.Run();
