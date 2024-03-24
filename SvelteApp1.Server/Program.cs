using Microsoft.EntityFrameworkCore;
using SvelteApp1.Server.Data;
using System.Security.Claims;
using SvelteApp1.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SvelteApp1.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = builder.Configuration;

            string connection = config.GetConnectionString("DefaultConnection")!;

            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IEmailConfirmationService, EmailConfirmationService>();
            builder.Services.AddTransient<RevokedJWTCashingService>();
            builder.Services.AddTransient<IAuthorizationHandler, CacheHandler>();
            builder.Services.AddTransient<QuestionMapperService>();
            
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            

            builder.Services.AddDbContext<ApplicationContext>(options => {
                options.UseMySql(connection,
                ServerVersion.AutoDetect(connection),
                mySqlOptions =>
                    mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
            );});

            builder.Services.AddControllers();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = config[JwtConfig.ConfigIssuer],
                    ValidateAudience = true,
                    ValidAudience = config[JwtConfig.ConfigAudience],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[JwtConfig.ConfigKey]!)),
                    ValidateIssuerSigningKey = true,
                };
            });

            builder.Services.AddAuthorization(opts => {
                opts.AddPolicy(JwtConfig.Polices.NotLoggedOut, policy => policy.Requirements.Add(new CacheRequirement()));
            });

            builder.Services.AddMemoryCache();

            builder.Services.AddResponseCompression(options => {
                options.EnableForHttps = true;
                options.Providers.Add(new GzipCompressionProvider(new GzipCompressionProviderOptions()));
            });

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Append("X-Xss-Protection", "1");
                context.Response.Headers.Append("X-Frame-Options", "DENY");

                await next();
            });
        
            app.Run();
        }
    }
}
