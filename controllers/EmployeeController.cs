using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NEmployee;
using Microsoft.AspNetCore.Http;
using Database;
using Npgsql;
using System.Collections.Generic;

namespace Payroll_Server {
   [Route("api/employee")]
   public class EmployeeController : Controller {

      [HttpGet]
      [Route("")]
      [Authorise(roles: "all")]
      public string FetchAllEmployees() {
         return JSON.Serializer<List<Employee>>(
            Connection.Sql<List<Employee>>($"SELECT * FROM {Table.EMPLOYEE}", func)
         );

         List<Employee> func(NpgsqlDataReader reader) {
            List<Employee> employeesList = new List<Employee>();

            while (reader.Read()) {
               employeesList.Add(
                  EmployeeManagement.UpdateReaderDataToEmployee(reader)
               );
            }

            return employeesList;
         }
      }
   }
}