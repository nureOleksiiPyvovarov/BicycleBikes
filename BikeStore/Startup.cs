using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BikeStore.Models;
using BikeStore.Models.Repos;
using BikeStore.Models.Repos.Interfaces;
using BikeStore.Profiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BikeStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddDbContext<BikeStoresContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BikeStoreConnection")));
            services.AddScoped<IProductsRepo, ProductsSqlRepo>();
            services.AddScoped<ICategoriesRepo, CategoriesSqlRepo>();
            services.AddScoped<IBrandsRepo, BrandsSqlRepo>();
            services.AddScoped<IOrdersRepo, OrdersSqlRepo>();
            services.AddScoped(ShoppingCart.GetCart);
            services.AddIdentity<UserIdentityModel, IdentityRole<int>>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<BikeStoresContext>();
            services.ConfigureApplicationCookie(options => { options.ExpireTimeSpan = TimeSpan.FromHours(1); });
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new BikesProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSession();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
