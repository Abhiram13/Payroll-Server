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
      private static int EmployeeCount(Employee employeeRequestBody) {
         return Connection.Sql<int>($"SELECT id FROM {Table.EMPLOYEE}", check);

         int check(Npgsql.NpgsqlDataReader reader) {
            List<long> list = new List<long>();

            while (reader.Read()) {
               if (employeeRequestBody.id == (long)reader[0]) {
                  list.Add((long)reader[0]);
               }
            }

            return list.Count;
         }
      }

      private static string InsertEmployee(string keys, string values) {
         string func(NpgsqlDataReader reader) => (reader.RecordsAffected == 1) ? "Successfully Added" : "Not Added";

         return Connection.Sql<string>($"INSERT INTO {Table.EMPLOYEE}({keys}) VALUES({values})", func);
      }
      
      public static async Task<string> AddEmployee(HttpRequest Request) {
         Employee employeeRequestBody = await JSON.httpContextDeseriliser<Employee>(Request);
         bool roleExist = isRoleExist(employeeRequestBody.designation);

         if (EmployeeCount(employeeRequestBody) > 0) return "Employee already Existed";

         if (!roleExist) return "Designation does not Exist";

         string values = OBJECT.GetValues<Employee>(employeeRequestBody);
         string keys = OBJECT.GetKeys<Employee>(employeeRequestBody);

         return InsertEmployee(keys, values);
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

      public static bool isRoleExist(string role) {
         bool isRoleExist = Connection.Sql<bool>($"SELECT role FROM {Table.ROLES} WHERE role ILIKE '{role}'", checkIfRoleExist);

         bool checkIfRoleExist(NpgsqlDataReader reader) => reader.HasRows;

         return isRoleExist;
      }
   }
}