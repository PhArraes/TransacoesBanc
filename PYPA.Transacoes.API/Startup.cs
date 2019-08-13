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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PYPA.Transacoes.DataMapping.Database;
using PYPA.Transacoes.DataMapping.Factories;
using PYPA.Transacoes.DataMapping.Interfaces;
using PYPA.Transacoes.Domain.Core;
using PYPA.Transacoes.Domain.Factories;
using PYPA.Transacoes.Domain.Interfaces.Core;
using PYPA.Transacoes.Facade;

namespace PYPA.Transacoes.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<DbConfiguration>(Configuration.GetSection("DbConfiguration"));
            
          //  services.AddScoped<IConnectionStringProvider, DbConfiguration>();
            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<TransacaoFactory>();
            services.AddScoped<TransactionFactory>();
            services.AddScoped<UsuarioRepositoryFactory>();
            services.AddScoped<ContaRepositoryFactory>();
            services.AddScoped<TransacaoRepositoryFactory>(); 
            services.AddScoped<ContaLockRepositoryFactory>();
            services.AddScoped<ContaLockService>();
            services.AddScoped<TransacaoService>();

            services.AddMvc();
            services.AddSingleton<IConfiguration>(Configuration);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<DbConfiguration> options)
        {
            new DatabaseInit(options).Init();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
