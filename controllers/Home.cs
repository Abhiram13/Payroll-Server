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
      [HttpDelete][HttpPut][HttpPost][HttpGet]
      public string Method() => "Welcome to Payroll";

      [HttpPost]
      [Route("login")]
      public void Login() { }

      [HttpPost]
      [Route("signup")]
      public async Task<string> Signup() => await EmployeeController.AddEmployee(Request);
   }
}