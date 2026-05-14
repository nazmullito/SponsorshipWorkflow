
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sponsorship.Application.Common;
using Sponsorship.Application.Interfaces;
using Sponsorship.Application.Validators;
using Sponsorship.Infrastructure.Authentication;
using Sponsorship.Infrastructure.Persistence;
using Sponsorship.Infrastructure.Services;
using System.Text;

namespace Sponsorship.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // JWT Settings
            builder.Services.Configure<JwtSettings>(
               builder.Configuration.GetSection("JwtSettings"));


            // Services
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateSponsorshipRequestValidator>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

            builder.Services.AddScoped<ISponsorshipRequestService, SponsorshipRequestService>();

            // JWT Authentication
            var jwtSettings = builder.Configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>();

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            ValidIssuer = jwtSettings!.Issuer,
                            ValidAudience = jwtSettings.Audience,

                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.Secret))
                        };
                });

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ApplicationDbContext>();

                var passwordHasher =
                    services.GetRequiredService<IPasswordHasher>();

                await context.Database.EnsureCreatedAsync();

                await DataSeeder.SeedAsync(context, passwordHasher);
            }

            app.Run();
        }
    }
}
