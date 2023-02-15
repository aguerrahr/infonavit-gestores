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
        private string _usarCifrado;
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
            _usarCifrado = Configuration.GetValue<string>("usarCifrado");


            if (_usarCifrado.Equals("true"))
            {
                //string _protected = Funciones.FuncionesUtiles.Encrypt("Data Source=LABINFO;Initial Catalog=Infonavit-Managers;Persist Security Info=True;User ID=sa;Password=31o5PhereAuth;");
                string _unprotected = Funciones.FuncionesUtiles.Decrypt(_generalThings);

                services.AddDbContext<AuthenticationAPIContext>(opt =>
                    opt.UseSqlServer(_unprotected));
            } else
            {
                services.AddDbContext<AuthenticationAPIContext>(opt =>
                    opt.UseSqlServer(_generalThings));
            }

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
