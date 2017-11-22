using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Com.DanLiris.Service.Core.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Com.DanLiris.Service.Core.Lib.Services;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Newtonsoft.Json.Serialization;

namespace Com.DanLiris.Service.Core.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? Configuration["DefaultConnection"];
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=com.danliris.db.core;Trusted_Connection=True;";
            services
                .AddDbContext<CoreDbContext>(options => options.UseSqlServer(connectionString))
                .AddTransient<BudgetService>()
                .AddTransient<BuyerService>()
                .AddTransient<CategoryService>()
                .AddTransient<CurrencyService>()
                .AddTransient<DivisionService>()
                .AddTransient<GarmentCurrencyService>()
                .AddTransient<StorageService>()
                .AddTransient<SupplierService>()
                .AddTransient<UnitService>()
                .AddTransient<UomService>()
                .AddTransient<VatService>()
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });


            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.ApiName = "com.danliris.service.core";
                    options.ApiSecret = "secret";
                    options.Authority = "https://localhost:44350";
                    options.RequireHttpsMetadata = false;
                    options.NameClaimType = JwtClaimTypes.Name;
                    options.RoleClaimType = JwtClaimTypes.Role;
                });

            services.AddCors(o => o.AddPolicy("CorePolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithExposedHeaders("Content-Disposition", "api-version", "content-length", "content-md5", "content-type", "date", "request-id", "response-time");
            }));

            services
                .AddMvcCore()
                .AddAuthorization(options =>
                {
                    options.AddPolicy("service.core.read", (policyBuilder) =>
                    {
                        policyBuilder.RequireClaim("scope", "service.core.read");
                    });
                })
                .AddJsonFormatters()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseCors("CorePolicy");

            app.UseMvc();
        }
    }
}
