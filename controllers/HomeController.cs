using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NEmployee;

namespace Payroll_Server {

   [Route("")]
   public class HomeController : Controller, IHomeController {

      [HttpGet]
      [HttpPost]
      [HttpPut]
      [HttpDelete]
      public string Method() => "Welcome to Payroll";

      [HttpPost]
      [Route("login")]
      public async Task<string> Login() => await EmployeeManagement.Login(Request);

      [HttpPost]
      [Route("signup")]
      public async Task<string> Signup() => await EmployeeManagement.Add(Request);
   }
}