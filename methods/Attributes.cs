using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc.Filters;
using Database;
using Npgsql;

namespace System {

   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class AuthoriseAttribute : ActionFilterAttribute {
      private string[] Roles;

      public AuthoriseAttribute(string roles) => this.Roles = roles.Split(",");

      private string fetchRole(string role) => Array.Find(this.Roles, r => r.ToUpper() == role.ToUpper());

      private string readerFunc(HttpResponse response, NpgsqlDataReader reader) {
         string role = "";
         while (reader.Read()) role = (string)reader[0];

         Console.WriteLine(role);

         string Role = fetchRole(role);
         bool isAll = this.Roles[0].ToUpper() == ("all").ToUpper();
         bool isNull = string.IsNullOrEmpty(Role);

         if (isAll) return "";

         if (string.IsNullOrEmpty(Role)) {
            response.StatusCode = 401;
            response.WriteAsync("You are not Authorised to access this Route");
            return "";
         }

         return "";
      }

      public override void OnActionExecuted(ActionExecutedContext context) {         
         HttpRequest request = context.HttpContext.Request;
         HttpResponse response = context.HttpContext.Response;
         StringValues token = request.Headers["Auth"];

         if (string.IsNullOrEmpty(token)) {
            response.StatusCode = 404;
            response.WriteAsync("Please provide valid Token");
            return;
         }

         string[] decode;

         try {
            decode = StringValue.Decode(token).Split(":");
            Connection.Sql($"SELECT designation FROM {Table.EMPLOYEE} WHERE id = {decode[0]}", VerifyUser);

            string VerifyUser(NpgsqlDataReader reader) => readerFunc(response, reader);

         } catch (Exception e) {
            response.StatusCode = 404;
            response.WriteAsync($"Token is InValid, {e.HResult}");
            return;
         }
      }
   }
}