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
   class EmployeeManagement {
      private static int Count(Employee employeeRequestBody) {
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

      private static string Insert(string keys, string values) {
         string func(NpgsqlDataReader reader) => (reader.RecordsAffected == 1) ? "Successfully Added" : "Not Added";

         return Connection.Sql<string>($"INSERT INTO {Table.EMPLOYEE}({keys}) VALUES({values})", func);
      }
      
      public static async Task<string> Add(HttpRequest Request) {
         Employee employeeRequestBody = await JSON.httpContextDeseriliser<Employee>(Request);
         bool roleExist = IsRoleExist(employeeRequestBody.designation);

         if (Count(employeeRequestBody) > 0) return "Employee already Existed";

         if (!roleExist) return "Designation does not Exist";

         string values = OBJECT.GetValues<Employee>(employeeRequestBody);
         string keys = OBJECT.GetKeys<Employee>(employeeRequestBody);

         return Insert(keys, values);
      }

      public static async Task<string> Login(HttpRequest request) {
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

      public static bool IsRoleExist(string role) {
         bool isRoleExist = Connection.Sql<bool>($"SELECT role FROM {Table.ROLES} WHERE role ILIKE '{role}'", checkIfRoleExist);

         bool checkIfRoleExist(NpgsqlDataReader reader) => reader.HasRows;

         return isRoleExist;
      }

      public static bool IsEmployeeExist(string id) {
         bool isEmployeeExist = Connection.Sql<bool>($"SELECT id FROM {Table.EMPLOYEE} WHERE id = {id}", checkIfRoleExist);
         bool checkIfRoleExist(NpgsqlDataReader reader) => reader.HasRows;

         return isEmployeeExist;
      }
   }
}