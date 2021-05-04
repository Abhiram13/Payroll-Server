using System;
using Npgsql;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using NEmployee;
using Database;

namespace NEmployee {
   class EmployeeController {
      public static async Task<string> AddEmployee(HttpRequest Request) {
         Employee emp = await JSON.httpContextDeseriliser<Employee>(Request);
         int employeeCount = Connection.Sql<int>($"SELECT id FROM {Table.EMPLOYEE}", check);

         int check(Npgsql.NpgsqlDataReader reader) {
            List<long> list = new List<long>();

            while (reader.Read()) {
               if (emp.id == (long)reader[0]) {
                  list.Add((long)reader[0]);
               }
            }

            return list.Count;
         }

         string func(NpgsqlDataReader reader) => (reader.RecordsAffected == 1) ? "Successfully Added" : "Not Added";

         if (employeeCount > 0) {
            return "Employee already Existed";
         }

         string values = OBJECT.GetValues<Employee>(emp);
         string keys = OBJECT.GetKeys<Employee>(emp);

         return Connection.Sql<string>($"INSERT INTO {Table.EMPLOYEE}({keys}) VALUES({values})", func);
      }

      public static async Task<string> EmployeeLogin(HttpRequest request) {
         Login login = await JSON.httpContextDeseriliser<Login>(request);
         string query = $"SELECT password, id, user_name FROM {Table.EMPLOYEE} WHERE id = {login.id} AND password = '{login.password}'";
         return Connection.Sql<string>(query, check);

         string check(NpgsqlDataReader reader) {
            while (reader.Read()) {
               return StringValue.Encode($"'{reader[1]}': {reader[2]}");
            }
            
            return "Employee Not Found";
         }
      }
   }
}