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
      [Authorise(roles:"Admin,HR")]
      public string fetchAll() {
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

      [HttpPost]
      [Route("add")]
      public async Task<string> add() {
         // string[] roles = new string[] { "Admin" };
         // ServerResponse token = Token.Verify(roles, Request.Headers);

         // if (token.Status != 200) {
         //    Response.StatusCode = token.Status;
         //    return token.Message;
         // }

         Role requestBody = await JSON.httpContextDeseriliser<Role>(Request);
         bool isRoleExist = Connection.Sql<bool>($"SELECT role FROM {Table.ROLES} WHERE role ILIKE '{requestBody.role}'", checkIfRoleExist);

         if (isRoleExist) {
            Response.StatusCode = 302;
            return "Role has already been Added";
         }

         int roleIncluded = Connection.Sql<int>($"INSERT INTO {Table.ROLES} (role) VALUES ('{requestBody.role}')", rowsAffected);

         if (roleIncluded > 0) {
            Response.StatusCode = 200;
            return "Role Successfully Added";
         }

         return "";

         bool checkIfRoleExist(NpgsqlDataReader reader) => reader.HasRows;

         int rowsAffected(NpgsqlDataReader reader) => reader.RecordsAffected;
      }
   }
}