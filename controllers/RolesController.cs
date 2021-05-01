using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NEmployee;
using Microsoft.AspNetCore.Http;
using Database;
using Npgsql;
using System.Collections.Generic;

namespace Payroll_Server {   

   [Route("api/roles")]
   public class RolesController : Controller {
      
      [HttpGet]
      [Route("all")]
      public string fetchAll() {
         string[] roles = new string[] { "Admin" };
         ServerResponse res = Token.Verify(roles, Request.Headers);

         if (res.Status != 200) {
            Response.StatusCode = res.Status;
            Response.WriteAsync(res.Message);
            return null;
         }

         string[] listOfRoles = Connection.Sql<string[]>($"SELECT * FROM {Table.ROLES}", fetchRoles);

         return JSON.Serializer<string[]>(listOfRoles);

         string[] fetchRoles(NpgsqlDataReader reader) {
            List<string> listOfroles = new List<string>();

            while (reader.Read()) {
               listOfroles.Add((string)reader[0]);
            }

            return listOfroles.ToArray();
         };
      }      
   }
}