using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using carlibraryapi.Model;
using Microsoft.EntityFrameworkCore;

namespace carlibraryapi
{
    public class Startup
    {
        readonly string AllowCORS = "AllowCORS"; //insert a global variable to reference the CORS configuration

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IConfigurationSection dbConfig = Configuration.GetSection("DBConnection");
            string connectionstring = "Host="+dbConfig["Host"]+";Port="+dbConfig["Port"]+";Username="+dbConfig["User"]+";Password="+dbConfig["Password"]+";Database="+dbConfig["Database"]+";";
            services.AddDbContext<CarLibraryContext>(options => options.UseNpgsql(connectionstring));

            //add a CORS service allowing all inbound traffic
            services.AddCors(options => {
                options.AddPolicy(name: AllowCORS, builder => {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); 
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "carlibraryapi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "carlibraryapi v1"));
            //}

            //app.UseHttpsRedirection(); //Disable this, to handle https outside of container

            app.UseRouting();

            app.UseCors(AllowCORS); //activate CORS

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
