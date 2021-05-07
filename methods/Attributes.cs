using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.Filters;
using Database;
using Npgsql;
using NEmployee;

// Decode Auth Header ::check
// Take id from decoded, check if such employee exist ::check
// if yes, then check if employee role is matched with the given roles in parameter
// if yes, authorise
// if parameter is all, then check if id is present in database
// if yes, then authorise

namespace System {

   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class AuthoriseAttribute : ActionFilterAttribute {
      private string[] Roles;

      public AuthoriseAttribute(string roles) {
         this.Roles = roles.Split(",");
      }

      private string FetchRole(string role) {
         return Array.Find(this.Roles, r => r.ToUpper() == role.ToUpper());
      }

      private string RoleFromReader(NpgsqlDataReader reader) {
         string role = "";
         while (reader.Read()) {
            role = (string)reader[0];
         }

         return role;
      }

      private bool AuthoriseAll(string role, bool isAll) {
         return (role != "" && isAll) ? true : false;
      }

      private string ReaderFunc(HttpResponse response, NpgsqlDataReader reader) {
         string roleFromReader = RoleFromReader(reader);
         string Role = FetchRole(roleFromReader);
         bool isAll = this.Roles[0].ToUpper() == ("all").ToUpper();
         bool isNull = string.IsNullOrEmpty(Role);

         if (AuthoriseAll(roleFromReader, isAll)) return "";

         if (string.IsNullOrEmpty(Role)) {
            Response(response, 401, "You are not Authorised to access this Route");
            return "";
         }

         return "";
      }

      private void Response(HttpResponse response, int statuCode, string message) {
         response.StatusCode = statuCode;
         response.WriteAsync(message);
      }

      public override void OnActionExecuted(ActionExecutedContext context) {
         HttpResponse response = context.HttpContext.Response;
         StringValues token = context.HttpContext.Request.Headers["Auth"];
         string[] decode;

         if (string.IsNullOrEmpty(token)) {
            Response(response, 404, "Please provide valid Token");
            return;
         }

         try {
            decode = StringValue.Decode(token).Split(":");
            Connection.Sql($"SELECT designation FROM {Table.EMPLOYEE} WHERE id = {decode[0]}", VerifyUser);

            string VerifyUser(NpgsqlDataReader reader) {
               return ReaderFunc(response, reader);
            }

         } catch (Exception e) {
            Response(response, 404, $"Token is InValid, {e.HResult}");
            return;
         }
      }
   }

   public class TestAttribute : ActionFilterAttribute {
      private string[] roles;
      private HttpResponse response;
      private Employee employee;

      public TestAttribute(string Roles) {
         roles = Roles.Split(",");
      }

      private void Response(int statuCode, string message) {
         response.StatusCode = statuCode;
         response.WriteAsync(message);
      }

      private bool IsRoleValid() {
         for (int i = 0; i < roles.Length; i++) {
            if (roles[i].ToUpper() == employee.designation.ToUpper()) return true;
         }

         return false;
      }

      public override void OnActionExecuted(ActionExecutedContext context) {
         response = context.HttpContext.Response;
         string id = new Token(context.HttpContext.Request).id;
         bool check = EmployeeManagement.IsEmployeeExist(id);
         employee = EmployeeManagement.GetEmployee(id);
      }
   }
}