using System;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using NEmployee;
using Database;
using Npgsql;
using System.Collections.Generic;

namespace Payroll_Server
{
   [Route("")]
   public class HomeController : Controller
   {
      [HttpGet]
      [HttpPost]
      [HttpPut]
      [HttpDelete]
      public async Task<string> method()
      {
         using (StreamReader reader = new StreamReader(Request.Body))
         {
            string body = await reader.ReadToEndAsync();
            Console.WriteLine(body);
         }     
         return "Welcome to Payroll";
      }

      [HttpPost]
      [Route("login")]
      public void login() { }

      [HttpPost]
      [Route("signup")]
      public async Task<string> signup()
      {
         return await EmployeeController.AddEmployee(Request);
      }
   }
}