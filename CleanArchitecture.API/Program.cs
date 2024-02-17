using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Identity;
using CleanArchitecture.API.Middleware;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registering my services by executing the services containers
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.ConfigureIdentityServices(builder.Configuration);

// Adding cors (generic in this case, just for practice purposes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

// stating that we will be using authentication
app.UseAuthentication();

app.UseAuthorization();

// Using the cors configuration created previously 
app.UseCors("CorsPolicy");

app.MapControllers();

// creating an instance of the current scope
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        // code to perform the migration, using classes and methods to create records
        var context = service.GetRequiredService<StreamerDbContext>();
        // asynchronously applies any pending migration for the context to the db. Will create the db if it does not already exist
        await context.Database.MigrateAsync();
        await StreamerDbContextSeed.SeedAsync(context, loggerFactory);
        await StreamerDbContextSeedData.LoadDataAsync(context, loggerFactory);

        // we are using two databases, so we gonna do the same with the Identity db
        var contextIdentity = service.GetRequiredService<CleanArchitectureIdentityDbContext>();
        await contextIdentity.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred while performing migration");
    }
}

    app.Run();
