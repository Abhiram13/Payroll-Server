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
   public class EmloyeeController : Controller {

      [HttpGet]
      [Route("")]
      [Authorise(roles: "all")]
      public void FetchAllEmployees() {
         // Employee func(NpgsqlDataReader reader) {
         //    while (reader.Read()) {
         //       return new Employee() {
         //          designation = (string)reader[5],
         //          email = (string)reader[4],
         //          first_name = (string)reader[1],
         //          id = (long)reader[0],
         //          last_name = (string)reader[2],
         //          mobile = (long)reader[3],
         //          password = (string)reader[8],
         //          supervisor = (long)reader[6],
         //          user_name = (string)reader[7]
         //       };
         //    }

         //    return new Employee();
         // }
      }
   }
}