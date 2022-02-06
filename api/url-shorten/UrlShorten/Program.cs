using Microsoft.EntityFrameworkCore;
using UrlShorten.Contexts;
using UrlShorten.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
var connectionString = builder.Configuration.GetConnectionString("Postgres");

// Controller services
builder.Services.AddControllers();
builder.Services.AddDbContext<UrlContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IAppContext>(provider => provider.GetService<UrlContext>());
builder.Services.AddScoped<IUrlValidator, UrlValidator>();
builder.Services.AddSingleton<IIdFactory, RandomIdFactory>();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors service
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins(allowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
