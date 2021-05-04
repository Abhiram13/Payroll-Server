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
         Response.StatusCode = StatusCodes.Status200OK;
         return JSON.Serializer<List<string>>(
            Connection.Sql<List<string>>($"SELECT * FROM {Table.ROLES}", fetchRoles)
         );

         List<string> fetchRoles(NpgsqlDataReader reader) {
            List<string> listOfroles = new List<string>();

            while (reader.Read()) listOfroles.Add((string)reader[0]);
            return listOfroles;
         };
      }

      [HttpPost]
      [Route("add")]
      [Authorise(roles: "Admin,HR")]
      public async Task<string> add() {
         Role requestBody = await JSON.httpContextDeseriliser<Role>(Request);
         bool isRoleExist = Connection.Sql<bool>($"SELECT role FROM {Table.ROLES} WHERE role ILIKE '{requestBody.role}'", checkIfRoleExist);

         if (isRoleExist) {
            Response.StatusCode = StatusCodes.Status304NotModified;
            return "Role has already been Added";
         }

         int roleIncluded = Connection.Sql<int>($"INSERT INTO {Table.ROLES} (role) VALUES ('{requestBody.role}')", rowsAffected);

         if (roleIncluded > 0) {
            Response.StatusCode = StatusCodes.Status201Created;
            return "Role Successfully Added";
         }

         Response.StatusCode = StatusCodes.Status502BadGateway;
         return "Internal Error";

         bool checkIfRoleExist(NpgsqlDataReader reader) => reader.HasRows;
         int rowsAffected(NpgsqlDataReader reader) => reader.RecordsAffected;
      }
   }
}