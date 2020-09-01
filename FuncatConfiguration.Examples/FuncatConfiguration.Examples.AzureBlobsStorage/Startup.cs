using System.Threading;
using FuncatConfiguration.Deserializer.Json;
using FuncatConfiguration.DI.MicrosoftDependencyInjection;
using FuncatConfiguration.Examples.Configurations;
using FuncatConfiguration.Storage.AzureBlobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FuncatConfiguration.Examples.AzureBlobsStorage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var azureBlobsConnectionString = Configuration.GetConnectionString("AzureBlobsConnectionString");

            var configurationManager = ConfigurationManagerBuilder
                .Create()
                .WithConfigurationType<SomeServiceConnectionSettings>() // Register SomeServiceConnectionSettings class as configuration class
                .WithConfigurationType<AnotherServiceConnectionSettings>() // Register SomeServiceConnectionSettings class as configuration class
                .WithJsonDeserializer() // Register Json serializer -- any deserializer registration required
                .WithAzureBlobsStorage(azureBlobsConnectionString, containerName: "testconfigs", "production") // User azure blobs as storage for configurations -- any storage registration required
                .BuildAsync(CancellationToken.None).Result;

            services.AddConfigurationTypes(configurationManager);
            services.AddControllers();
        }
    }
}