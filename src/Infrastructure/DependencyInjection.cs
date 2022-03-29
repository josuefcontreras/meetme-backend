using Application.Common.Interfaces;
using Domain;
using Infrastructure.Persistence;
using Infrastructure.Photo;
using Infrastructure.Security;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseSqlite"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
            }
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chat")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
            services.AddScoped<TokenService>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.Configure<GoogleCloudStorageSettings>(configuration.GetRequiredSection("GoogleCloudStorage"));
            services.AddSignalR();

            return services;
        }

    }
}
