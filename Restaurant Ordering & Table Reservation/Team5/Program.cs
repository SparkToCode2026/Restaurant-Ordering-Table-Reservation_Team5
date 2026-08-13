using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Team5;
using Team5.Services;

namespace Team5
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //DbContext
            builder.Services.AddDbContext<ProjectContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            ////////////////
            ///
            builder.Services.AddControllers(options =>
            {
                // Navigation properties (e.g. MenuCategory.MenuItems) are non-nullable
                // reference types, which ASP.NET Core would otherwise treat as implicitly
                // [Required] during model validation. That would make it impossible to
                // create/update an entity without also sending its related collections.
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // =====================================
            // JWT Authentication
            // =====================================

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    )
                };
            });


            // =====================================
            // Authorization
            // =====================================

            builder.Services.AddAuthorization();


            // =====================================
            // Email Service
            // =====================================

            builder.Services.AddScoped<EmailService, EmailService>();
            // builder.Services.AddScoped<EmailService, EmailService>(); same as builder.Services.AddScoped<EmailService>();

            // JWT token generator (required by AuthController)
            builder.Services.AddScoped<JwtService>();

            // =====================================
            // Swagger
            // =====================================

            //swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the box below"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            new List<string>()
        }
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

            app.UseCors("FrontendPolicy");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}