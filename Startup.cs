using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NEmployee;
using Database;
using Npgsql;

namespace Payroll_Server
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
         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payroll_Server", Version = "v1" });
         });
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payroll_Server v1"));
         }

         app.UseHttpsRedirection();

         app.UseRouting();

         app.UseAuthorization();         

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();

            // endpoints.MapGet("/employee/roles/all", (HttpContext context) => 
            // {               
            //    return context.Response.WriteAsync(Connection.Sql($"SELECT * FROM {Table.ROLES}", Roles.fetchAll));
            // });

            // endpoints.MapPost("/employee/add", async (HttpContext context) =>
            // {
            //    EmployeeController controller = new EmployeeController(context);
            //    string keys = controller.keys();
            //    string values = await controller.values();
            //    await context.Response.WriteAsync(Connection.Sql($"INSERT INTO {Table.EMPLOYEE}({keys}) VALUES({values})", EmployeeController.check));
            // });
         });
      }
   }
}
