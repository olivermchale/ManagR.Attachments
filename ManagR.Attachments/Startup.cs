using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagR.Attachments.Data;
using ManagR.Attachments.Repository;
using ManagR.Attachments.Repository.Interfaces;
using ManagR.Attachments.Services;
using ManagR.Attachments.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;


namespace ManagR.Attachments
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)

        {
            Configuration = configuration;
            Environment = environment;
        }
        public IConfiguration Configuration { get; }

        private IHostingEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("ManagRAppServices",
                builder =>
                {
                    builder.WithOrigins("https://localhost:4200",
                                        "http://localhost:4200")
                                        .AllowAnyOrigin()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader();
                });
            });

            services.AddDbContext<AttachmentsDb>(options => options.UseSqlServer(
                Configuration.GetConnectionString("Attachments")));

            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IAttachmentsRepository, AttachmentsRepository>();

            services.AddControllers().AddNewtonsoftJson(options =>
                     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("ManagRAppServices");

            app.UseEndpoints(endpoints =>
            {
                // default controller notation
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
