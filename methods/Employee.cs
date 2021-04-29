using System;
using Npgsql;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NEmployee
{
   class Roles
   {
      public static string fetchAll(NpgsqlDataReader reader)
      {
         List<string> list = new List<string>();

         while (reader.Read())
         {
            list.Add((string)reader[0]);
         }

         return JsonSerializer.Serialize(list.ToArray());
      }
   }

   public class Employee
   {
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string email { get; set; }
      public string designation { get; set; }
      public string user_name { get; set; }
      public string password { get; set; }
      public long supervisor { get; set; }
      public long mobile { get; set; }
      public long id { get; set; }
   }

   class EmployeeController
   {
      Task<Employee> employee;
      public EmployeeController(HttpContext context)
      {
         this.employee = JSON.httpContextDeseriliser<Employee>(context);
      }

      public async Task<string> values()
      {
         Employee emp = await this.employee;
         return $"'{emp.first_name}', '{emp.last_name}', '{emp.designation}', '{emp.email}', {emp.mobile}, {emp.id}, {emp.supervisor}";
      }

      public string keys()
      {
         return "first_name, last_name, designation, email, mobile, id, supervisor";
      }

      public static string check(NpgsqlDataReader reader)
      {
         Console.WriteLine(reader.RecordsAffected);
         return "agahjsd";
      }
   }
}