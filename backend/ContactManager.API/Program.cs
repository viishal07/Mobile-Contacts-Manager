using ContactManager.API.Data;
using ContactManager.API.Interfaces;
using ContactManager.API.Mappings;
using ContactManager.API.Middleware;
using ContactManager.API.Repositories;
using ContactManager.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//  Database 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//  AutoMapper 
builder.Services.AddAutoMapper(typeof(ContactProfile));

//  DI Registration 
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

//  CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "Contact Manager API",
        Version = "v1",
        Description = "A RESTful API for managing contacts"
    });
});

//  Logging 
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

//  Middleware Pipeline 
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact Manager API v1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.MapControllers();

//  Auto-migrate on startup 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Migration failed. Ensure the database is reachable.");
    }
}

app.Run();
