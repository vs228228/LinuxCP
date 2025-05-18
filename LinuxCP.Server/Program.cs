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


            // �������� ������ �����������
            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb");

            // ������ �������
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase("logs_db");

            // ��������� Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.MongoDB(database, collectionName: "logs") // ���������� IMongoDatabase
                .Enrich.FromLogContext()
                .CreateLogger();

            // ���������� Serilog � �����
            builder.Host.UseSerilog();





            // Add DbContext
            builder.Services.AddDbContext<PostgresDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

            var app = builder.Build();

            // Enabling Swagger only in development mode
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
