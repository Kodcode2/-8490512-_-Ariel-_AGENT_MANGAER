using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MosadAPIServer.Data;
using MosadAPIServer.Services;

namespace MosadAPIServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MosadAPIServerContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MosadAPIServerContext")
                ?? throw new InvalidOperationException("Connection string 'MosadAPIServerContext' not found.")));

            // Add services to the container.
            builder.Services.AddScoped<AgentService>();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
