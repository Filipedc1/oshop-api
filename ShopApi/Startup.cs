using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ShopApi.Data;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using ShopApi.Services;
using System.Text;

[assembly: ApiController]
namespace ShopApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure swagger generator
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo() { Title = "PizzaMia API", Version = "v1" });
            });

            // Allows angular app which is hosted on a different server/domain to communicate with the api
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            // Add JWT Authentication
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

            services.AddAuthorization(opt =>
            {
                //policy.AddAuthenticationSchemes("Cookie, Bearer");
                opt.AddPolicy("Admin", policy => { policy.RequireClaim("role", "Admin"); policy.RequireAuthenticatedUser(); });
                opt.AddPolicy("Customers", policy => { policy.RequireClaim("role", "Customer"); policy.RequireAuthenticatedUser(); });
            });

            // Auto Mapper Configuration
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfiguration());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<IProductService, ProductService>();

            services.AddControllers()
                    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaMia API v1");
                opt.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Allows angular app which is hosted on a different server/domain to communicate with the api
                app.UseCors("CorsPolicy");
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
