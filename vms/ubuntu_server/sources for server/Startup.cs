using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic;
using DataAccess;
using Domain.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RestApi.Middleware;
using RestApi.Validators;
using Serilog;

namespace RestApi
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
            services.AddControllers();
            services.AddFluentValidation();

            services
                .AddBusinessLogic()
                .AddDataAccess(Configuration.GetConnectionString("PersonDb"));

            services.AddTransient<IValidator<Student>, StudentValidator>()
                .AddTransient<IValidator<Lector>, LectorValidator>()
                .AddTransient<IValidator<Lection>, LectionValidator>()
                .AddTransient<IValidator<Homework>, HomeworkValidator>()
                .AddTransient<IValidator<LectionLog>, LectionLogValidator>();

            // services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "RestApi", Version = "v1"}); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestApi v1"));
            }

            // app.UseHttpsRedirection();
            
            app.UseSerilogRequestLogging();
            
            // to catch errors
            app.UseMiddleware<ErrorHandlingMiddleware>(); 

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}