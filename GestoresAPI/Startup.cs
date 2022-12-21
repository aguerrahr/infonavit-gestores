using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

using GestoresAPI.Models.Contexts;
using Swashbuckle.AspNetCore.SwaggerGen;
using GestoresAPI.Filters;
using System;


namespace GestoresAPI
{
    public class Startup
    {
        private static readonly string AUTHORIZATION_BASE_API = "api/Authentication";
        private string _generalThings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            _generalThings = Configuration["CON_ENVIRONMENT"];
            services.AddDbContext<GestoresAPIContext>(opt =>
                                               opt.UseSqlServer(_generalThings));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestoresAPI", Version = "v1" });
                c.OperationFilter<AuthorizationHeaderRequired>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestoresAPI v1"));
            }
            

//            app.UseHttpsRedirection();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)                                     
                .AllowCredentials()
                .WithExposedHeaders("Authorization")
            );                        
            string AUTHORIZATION_DOMAIN = this.Configuration.GetConnectionString("AUTHORIZATION_DOMAIN");
            app.UseOAuthEndpoint(AUTHORIZATION_DOMAIN, AUTHORIZATION_BASE_API, new String[] { "api/Settings" });            
            //app.UseOAuthEndpoint(AUTHORIZATION_DOMAIN, AUTHORIZATION_BASE_API, new String[] { "api/SignPayload" });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
