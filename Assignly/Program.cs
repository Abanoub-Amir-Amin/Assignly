using Assignly.Infrastructure;
using Assignly.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assignly
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDBContext>(op =>
                op.UseSqlServer(builder.Configuration.GetConnectionString("AssignlyDB")));

            //builder.Services.AddScoped<IGenericRepository, GenericRepository>();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}
