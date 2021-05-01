using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NEmployee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Database;
using Microsoft.Extensions.Primitives;

namespace Payroll_Server {   

   [Route("api/roles")]
   public class RolesController : Controller {
      
      [HttpGet]
      [Route("all")]
      public void fetchAll() {
         string[] roles = new string[] { "HR" };
         ServerResponse res = Token.Verify(roles, Request.Headers);
         Response.StatusCode = res.Status;
         Response.WriteAsync(res.Message);
      }      
   }
}