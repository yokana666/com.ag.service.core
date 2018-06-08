using Com.DanLiris.Service.Core.Lib;
using Com.DanLiris.Service.Core.Lib.Services;
using Com.DanLiris.Service.Core.Test.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Com.DanLiris.Service.Core.Test
{
    public class ServiceProviderFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public ServiceProviderFixture()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? configuration["DefaultConnection"];

            this.ServiceProvider = new ServiceCollection()
                .AddDbContext<CoreDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Transient)
                .AddTransient<BudgetService>(provider => new BudgetService(provider))
                .AddTransient<BudgetServiceDataUtil>()
                .AddTransient<ComodityService>(provider => new ComodityService(provider))
                .AddTransient<ComodityServiceDataUtil>()
                .AddTransient<QualityService>(provider => new QualityService(provider))
                .AddTransient<QualityServiceDataUtil>()
                .AddTransient<YarnMaterialService>(provider => new YarnMaterialService(provider))
                .AddTransient<YarnMaterialServiceDataUtil>()
                .AddTransient<MaterialConstructionService>(provider => new MaterialConstructionService(provider))
                .AddTransient<MaterialConstructionServiceDataUtil>()
                .BuildServiceProvider();

            CoreDbContext dbContext = ServiceProvider.GetService<CoreDbContext>();
            dbContext.Database.Migrate();
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("ServiceProviderFixture Collection")]
    public class ServiceProviderFixtureCollection : ICollectionFixture<ServiceProviderFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}