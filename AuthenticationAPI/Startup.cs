using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using AuthenticationAPI.Models.Contexts;
using System.Net.Http;

namespace AuthenticationAPI
{
    public class Startup
    {
        private string _generalThings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            _generalThings = Configuration["CON_ENVIRONMENT"];            
            //services.AddSingleton<SomeClass>();
            services.AddControllers();            
            services.AddDbContext<AuthenticationAPIContext>(opt =>
                opt.UseSqlServer(_generalThings));
			//services.AddDbContext<AuthenticationAPIContext>(opt =>
   //             opt.UseSqlServer(Configuration.GetConnectionString("ConnectionDBGestores")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticationAPI", Version = "v1" });
            });           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthenticationAPI v1"));
            }
            //app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
}
