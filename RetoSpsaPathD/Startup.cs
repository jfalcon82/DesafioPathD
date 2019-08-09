using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using RetoSpsaPathD.Models;

namespace RetoSpsaPathD
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
            //agregar
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("paisDB"));
            services.AddMvc().AddJsonOptions(ConfigureJson);
            services.AddSwaggerGen(config => config.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info()
            {
                Title = "Documentacion de API PathD  (Javier Falcon)",
                Description = "Documentacion brebe para el desafio del PathD",
                Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                {
                    Name = "Javier Falcon",
                    Email = "javier.falcon@spsa.pe"
                    
                    

                

                }
            })
            
            );

            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "RetoSpsaPathD.xml");

            //RetoSpsaPathD\RetoSpsaPathD\RetoSpsaPathD.xml
            services.AddSwaggerGen(config => config.IncludeXmlComments(filePath));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void ConfigureJson(MvcJsonOptions obj)
        {
            //ignora la referencia ciclica
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
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

            if (!context.Clientes.Any())
            {
                context.Clientes.AddRange(new List<Cliente>()
                {
                   new Cliente(){Nombre = "Juan", Apellido ="Pérez", FechaNacimiento = new DateTime(1973,1,10), RandomDiasVida= new Random().Next(25000, 37000) },
                   new Cliente(){Nombre = "Maria", Apellido = "Guzmán", FechaNacimiento = new DateTime(1994,5,15), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Luis", Apellido = "Rojas", FechaNacimiento = new DateTime(1974,7,17), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Pedro", Apellido ="Villar", FechaNacimiento = new DateTime(2014,8,25), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Lucia", Apellido = "Osorio", FechaNacimiento = new DateTime(1990,01,22), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Pamela", Apellido = "Molina", FechaNacimiento = new DateTime(1974,03,20), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Ana", Apellido ="Casas", FechaNacimiento = new DateTime(2001,5,7), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Gabriel", Apellido = "Fernández", FechaNacimiento = new DateTime(2004,10,12), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Mario", Apellido = "Del Prado", FechaNacimiento = new DateTime(1991,12,17), RandomDiasVida= new Random().Next(25000, 37000)},
                   new Cliente(){Nombre = "Estefany", Apellido = "Massa", FechaNacimiento = new DateTime(1973,2,27), RandomDiasVida= new Random().Next(25000, 37000)}

                });

                context.SaveChanges();
            }

            app.UseSwagger();
            app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "Api documentada en SWAGGER"));



        }
    }
}
