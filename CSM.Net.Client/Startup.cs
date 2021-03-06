﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CSM
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
			services.AddCors(o =>
			{
				o.AddPolicy("Everything", p =>
				{
					p.AllowAnyHeader()
					 .AllowAnyMethod()
					 .AllowAnyOrigin();
				});
			});

			services.AddMvc();

			services.AddScoped<ITableRepository, TableRepository>();
			services.AddScoped<IInvoiceRepository, InvoiceRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
			}

			app.UseCors("Everything");
			app.UseMvc();
		}
    }
}
