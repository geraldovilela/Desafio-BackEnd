using MassTransit;
using Microsoft.EntityFrameworkCore;
using RentalApp.Core.AutoMapper;
using RentalApp.Core.Services;
using RentalApp.Core.Services.IServices;
using RentalApp.Infrastructure.DBContext;
using RentalApp.Infrastructure.Repositories;
using RentalApp.Infrastructure.Repositories.Interfaces;
using RentalApp.Infrastructure.Repositories.Interfaces.RentalApp.Core.Interfaces;
using RentalApp.Messaging.Config;
using RentalApp.Messaging.Consumer;
using RentalApp.Messaging.Health;
using RentalApp.Messaging.Interface;
using RentalApp.Messaging.Publisher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RentalDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMotorcyclesService, MotorcyclesService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IRenterRepository, RenterRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IRabbitMQHealthService, RabbitMQHealthService>();

builder.Services.AddAutoMapper(cfg => {
    cfg.AddProfile<MotorcycleProfile>();
});

builder.Services.Configure<RabbitMQConfig>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<Motorcycle2024Consumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = builder.Configuration["RabbitMQ:HostName"];
        var rabbitPort = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672");
        var rabbitUser = builder.Configuration["RabbitMQ:UserName"];
        var rabbitPass = builder.Configuration["RabbitMQ:Password"];
        var rabbitVHost = builder.Configuration["RabbitMQ:VirtualHost"];

        cfg.Host(rabbitHost, (ushort)rabbitPort, rabbitVHost, h =>
        {
            h.Username(rabbitUser);
            h.Password(rabbitPass);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await ApplyMigrationsAsync(app);

if (app.Environment.IsDevelopment() ||
    app.Environment.EnvironmentName == "Docker" ||
    Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Health check endpoints
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
}));

app.MapGet("/health/rabbitmq", async (IRabbitMQHealthService rabbitHealthService) =>
{
    var healthInfo = await rabbitHealthService.GetHealthInfoAsync();

    if (healthInfo.IsHealthy)
    {
        return Results.Ok(healthInfo);
    }
    else
    {
        return Results.Problem(
            detail: healthInfo.ErrorMessage ?? "RabbitMQ connection failed",
            statusCode: 503,
            title: "Service Unavailable"
        );
    }
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Uncomment if you need to start message consumer
// var consumer = app.Services.GetRequiredService<IMessageConsumer>();
// consumer.StartConsuming();

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<RentalDBContext>();

        logger.LogInformation("Checking database connection...");

        var maxRetries = 60;
        var delay = TimeSpan.FromSeconds(1);

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                await context.Database.CanConnectAsync();
                logger.LogInformation("Database connection established");
                break;
            }
            catch (Exception ex) when (i < maxRetries - 1)
            {
                logger.LogWarning("Attempting to connect to database... Attempt {Attempt}/{MaxRetries}", i + 1, maxRetries);
                await Task.Delay(delay);
            }
        }

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            logger.LogInformation("Applying {Count} pending migrations", pendingMigrations.Count());
            foreach (var migration in pendingMigrations)
            {
                logger.LogInformation("Pending migration: {Migration}", migration);
            }

            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully");
        }
        else
        {
            logger.LogInformation("No pending migrations found");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error applying migrations: {ErrorMessage}", ex.Message);

        if (app.Environment.IsProduction())
        {
            logger.LogCritical("Critical failure in production - application will terminate");
            throw;
        }
        else
        {
            logger.LogWarning("Continuing execution despite migration error (development environment)");
        }
    }
}