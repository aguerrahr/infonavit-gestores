using Microsoft.AspNetCore.DataProtection;
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
using System;

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
            _generalThings =  Configuration["CON_ENVIRONMENT"];
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var servicios = serviceCollection.BuildServiceProvider();

            // get an IDataProtector from the IServiceProvider
            var _protector = servicios.GetDataProtector("GestoresAPI");

            //_generalThings = "Data Source=LABINFO;Initial Catalog=Infonavit-Managers;Persist Security Info=True;User ID=sa;Password=31o5PhereAuth;";


            //string protectedPayload = _protector.Protect(_generalThings);

            string unprotectedPayload = _protector.Unprotect(_generalThings);
            
            //services.AddSingleton<SomeClass>();
            services.AddControllers();            
            services.AddDbContext<AuthenticationAPIContext>(opt =>
                opt.UseSqlServer(unprotectedPayload));
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
