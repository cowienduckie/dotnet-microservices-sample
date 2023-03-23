using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataService;
using PlatformService.Data;
using PlatformService.SyncDataService.Grpc;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    Console.WriteLine($"--> Using MSSQL DB: {builder.Configuration.GetConnectionString("PlatformsConn")}");

    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
    );
}
else
{
    Console.WriteLine("--> Using InMem DB");

    builder.Services.AddDbContext<AppDbContext>(
        opt => opt.UseInMemoryDatabase("InMem")
    );
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddScoped<IMessageBusClient, MessageBusClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddGrpc();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            builder.Configuration["RabbitmqHost"],
            h =>
            {
                h.Username(builder.Configuration["RabbitmqUsername"]);
                h.Password(builder.Configuration["RabbitmqPassword"]);
            });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcPlatformService>();

app.MapGet("/protos/platforms.proto",
    async context =>
    {
        await context.Response.WriteAsync(await File.ReadAllTextAsync("../Shared/Protos/platforms.proto"));
    });

app.PrePopulation(app.Environment.IsProduction());

app.Run();