using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShopApi.Data;
using ShopApi.Core.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Core;
using System;
using System.Text;
using ShopApi.Core.Services;
using ShopApi.Core.Interfaces;
using System.Text.Json.Serialization;

[assembly: ApiController]
namespace ShopApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddRouting(opts =>
            {
                opts.LowercaseUrls = true;
                opts.LowercaseQueryStrings = true;
            });

            // Allows angular app which is hosted on a different server/domain to communicate with the api
            services.AddCors(opt => opt.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin()
                                                                      .AllowAnyHeader()
                                                                      .AllowAnyMethod()));

            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            RegisterAuth(services);

            // Auto Mapper Configuration
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfiguration());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Redis")["DefaultConnection"];
                options.InstanceName = Configuration.GetSection("Redis")["Instance"];
            });

            RegisterServices(services);
            AddSwagger(services);

            services.AddResponseCompression(x => x.EnableForHttps = true);

            services.AddControllers()
                    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop API v1");
                    opt.RoutePrefix = string.Empty;
                });

                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/heath");
                endpoints.MapControllers();
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderService, OrderService>();
        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo() { Title = "Shop API", Version = "v1" });

                // Add authentication to swagger
                var bearerScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Name = "Authorization",
                    Scheme = "bearer",
                    Description = "Enter JWT token bellow",
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                opts.AddSecurityDefinition("Bearer", bearerScheme);
                opts.AddSecurityRequirement(new OpenApiSecurityRequirement() { { bearerScheme, new string[] { } } });
            });
        }

        private void RegisterAuth(IServiceCollection services)
        {
            services.AddIdentityCore<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(opt =>
                    {
                        opt.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Jwt:JwtIssuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["Jwt:JwtIssuer"],
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtKey"])),
                            RequireSignedTokens = true,

                            // Ensure the token hasn't expired:
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                        };
                    });

            //services.AddAuthorization(opt =>
            //{
            //    opt.AddPolicy("Admin", policy => { policy.RequireClaim("role", "Admin"); policy.RequireAuthenticatedUser(); });
            //    opt.AddPolicy("Customers", policy => { policy.RequireClaim("role", "Customer"); policy.RequireAuthenticatedUser(); });
            //});
        }
    }
}
