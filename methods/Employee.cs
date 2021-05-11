using System;
using Npgsql;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Database;

namespace NEmployee {
   public class EmployeeManagement {
      private static bool EmployeeExist(Employee employeeRequestBody) {
         const string QUERY = "SELECT id FROM " + Table.EMPLOYEE;
         
         bool IsEmployeeExist(NpgsqlDataReader reader) {
            while (reader.Read()) {
               if (employeeRequestBody.id == (long)reader[0]) {
                  return true;
               }
            }

            return false;
         }

         return Connection.Sql<bool>(QUERY, IsEmployeeExist);
      }

      private static string Insert(string keys, string values) {
         return Connection.Sql<string>(
            $"INSERT INTO {Table.EMPLOYEE}({keys}) VALUES({values})",
            (reader) => (reader.RecordsAffected == 1) ? "Successfully Added" : "Not Added"
         );
      }
      
      public static async Task<string> Add(HttpRequest Request) {
         Employee employeeRequestBody = await JSON.httpContextDeseriliser<Employee>(Request);
         bool roleExist = IsRoleExist(employeeRequestBody.designation);

         if (EmployeeExist(employeeRequestBody)) return "Employee already Existed";

         if (!roleExist) return "Designation does not Exist";

         string values = OBJECT.GetValues<Employee>(employeeRequestBody);
         string keys = OBJECT.GetKeys<Employee>(employeeRequestBody);

         return Insert(keys, values);
      }

      public static async Task<string> Login(HttpRequest request) {
         Login login = await JSON.httpContextDeseriliser<Login>(request);
         string query = $"SELECT password, id, user_name FROM {Table.EMPLOYEE} WHERE id = {login.id} AND password = '{login.password}'";
         return Connection.Sql<string>(query, (reader) => {
            while (reader.Read()) {
               return StringValue.Encode($"'{reader[1]}': {reader[2]}");
            }

            return "Employee Not Found";
         });
      }

      public static bool IsRoleExist(string role) {
         return Connection.Sql<bool>(
            $"SELECT role FROM {Table.ROLES} WHERE role ILIKE '{role}'", 
            (reader) => reader.HasRows
         );
      }

      public static bool IsEmployeeExist(string id) {
         return Connection.Sql<bool>(
            $"SELECT id FROM {Table.EMPLOYEE} WHERE id = {id}",
            (reader) => reader.HasRows
         );
      }

      public static Employee UpdateReaderDataToEmployee(NpgsqlDataReader reader) {
         return new Employee() {
            designation = (string)reader[5],
            email = (string)reader[4],
            first_name = (string)reader[1],
            id = (long)reader[0],
            last_name = (string)reader[2],
            mobile = (long)reader[3],
            password = (string)reader[8],
            supervisor = (long)reader[6],
            user_name = (string)reader[7]
         };
      }      

      public static Employee GetEmployee(string id) {
         string query = $"SELECT * FROM {Table.EMPLOYEE} WHERE id = {id}";

         return Connection.Sql(query, func);

         Employee func(NpgsqlDataReader reader) {
            while (reader.Read()) {
               return UpdateReaderDataToEmployee(reader);
            }

            return new Employee();
         }
      }
   }
}