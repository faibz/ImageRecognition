using DistributedSystems.API.Adapters;
using DistributedSystems.API.Factories;
using DistributedSystems.API.Repositories;
using DistributedSystems.API.Services;
using DistributedSystems.API.Utils;
using DistributedSystems.API.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DistributedSystems.API
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
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            services.AddTransient<IImagesRepository, ImagesRepository>();
            services.AddTransient<ITagsRepository, TagsRepository>();
            services.AddTransient<IMapsRepository, MapsRepository>();
            services.AddTransient<IWorkerClientVersionsRepository, WorkerClientVersionsRepository>();
            services.AddTransient<ICompoundImageMappingsRepository, CompoundImageMappingsRepository>();
            services.AddTransient<ICompoundImagesRepository, CompoundImagesRepository>();
            services.AddTransient<ICompoundImageTagsRepository, CompoundImageTagsRepository>();

            services.AddTransient<IMapsValidator, MapsValidator>();
            services.AddTransient<IImagesValidator, ImagesValidator>();
            services.AddTransient<ITagsValidator, TagsValidator>();

            services.AddTransient<IMapsAnalyser, MapsAnalyser>();
            services.AddTransient<ITagsAnalyser, TagsAnalyser>();

            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<ITagsService, TagsService>();
            services.AddTransient<IMapsService, MapsService>();
            services.AddTransient<IWorkerClientVersionsService, WorkerClientVersionsService>();

            services.AddSingleton<IFileStorageAdapter, AzureBlobStorageAdapter>();
            services.AddTransient<IQueueAdapter, ServiceBusAdapter>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
