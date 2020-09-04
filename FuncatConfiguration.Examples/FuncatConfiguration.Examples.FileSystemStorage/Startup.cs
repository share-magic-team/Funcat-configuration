using System.Threading;
using FuncatConfiguration.Deserializer.Json;
using FuncatConfiguration.DI.MicrosoftDependencyInjection;
using FuncatConfiguration.Examples.Configurations;
using FuncatConfiguration.Storage.FileSystem;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FuncatConfiguration.Examples.FileSystemStorage
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
            var configurationManager = ConfigurationManagerBuilder
                .Create()
                .WithConfigurationType<SomeServiceConnectionSettings>() // Register SomeServiceConnectionSettings class as configuration class
                .WithConfigurationType<AnotherServiceConnectionConfiguration>() // Register SomeServiceConnectionSettings class as configuration class
                .WithJsonDeserializer() // Register Json serializer -- any deserializer registration required
                .WithFileSystemStorage(folder: "ProdConfigurations") // User file system as storage for configurations -- any storage registration required
                .BuildAsync(CancellationToken.None).Result;

            // Get config explicitly
            var someServiceConfig = configurationManager.GetConfigurationAsync<SomeServiceConnectionSettings>(CancellationToken.None).Result;
            var anotherServiceConfig = configurationManager.GetConfigurationAsync<AnotherServiceConnectionConfiguration>("AnotherServiceConnection", CancellationToken.None).Result;

            services.AddConfigurationTypes(configurationManager);
            services.AddControllers();
        }
    }
}