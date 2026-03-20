using System.Text;
using Assignly.Data.Enums;
using Assignly.Data.Models;
using Assignly.Infrastructure;
using Assignly.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Assignly
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDBContext>(op =>
                op.UseSqlServer(builder.Configuration.GetConnectionString("AssignlyDB"))
            );
            builder
                .Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddServicesDI();

            builder.Services.AddInfrastructureDI();
            var jwtSettings = builder.Configuration.GetSection("Jwt");

            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "JwtBearer";
                    options.DefaultChallengeScheme = "JwtBearer";
                })
                .AddJwtBearer(
                    "JwtBearer",
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings["Issuer"],
                            ValidAudience = jwtSettings["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings["Key"])
                            ),
                            ClockSkew = TimeSpan.Zero, // optional (removes default 5 min delay)
                        };
                    }
                );

            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<
                    RoleManager<IdentityRole>
                >();
                await SeedRoles(roleManager);
            }
            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UsePathBase("/swagger/index.html");

            app.MapControllers();

            app.Run();
        }

        public static async System.Threading.Tasks.Task SeedRoles(
            RoleManager<IdentityRole> roleManager
        )
        {
            foreach (var role in Enum.GetNames(typeof(RoleEnum)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
