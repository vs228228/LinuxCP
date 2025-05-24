using LinuxCP.Application.Interfaces;
using LinuxCP.Infrastructure.Persistence.Contexts;
using LinuxCP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;

namespace LinuxCP.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient<IOllamaService, OllamaService>();

            // Получаем строку подключения к MongoDB
            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase("logs_db");

            // Настройка Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MongoDB(database, collectionName: "logs")
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();

            // Add DbContext
            builder.Services.AddDbContext<PostgresDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalAndAngular", policy =>
                {
                    policy.WithOrigins(
                        "https://localhost:7053",  // твой backend origin, если фронтенд и бэкенд на одном порте
                        "http://localhost:7053",   // если фронтенд без https (редко, но на всякий)
                        "https://127.0.0.1:4200", // Angular dev server с https
                        "http://127.0.0.1:4200",  // Angular dev server с http
                        "http://localhost:4200",  // Angular dev server с http + localhost
                        "https://localhost:4200"  // Angular dev server с https + localhost
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });


            var app = builder.Build();

            // Enable Swagger в dev-среде
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("AllowLocalAndAngular");

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
