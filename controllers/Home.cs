using System;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_Server
{
   [Route("")]
   public class HomeController : Controller
   {
      [HttpGet]
      [HttpPost]
      public string method()
      {
         return "Welcome to Payroll";
      }

      [HttpPost]
      [Route("login")]
      public void login()
      {

      }

      [HttpPost]
      [Route("signup")]
      public void signup()
      {

      }
   }
}