using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Npgsql;
using Database;
using Microsoft.Extensions.Primitives;

namespace System
{
   [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
   public class AuthoriseAttribute : ActionFilterAttribute {
      private string[] Roles;

      public AuthoriseAttribute(string roles) => this.Roles = roles.Split(",");

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

            string VerifyUser(Npgsql.NpgsqlDataReader reader) {
               string role = "";
               while (reader.Read()) role = (string)reader[0];

               string Role = Array.Find(this.Roles, r => r.ToUpper() == role.ToUpper());

               if (Role == null || string.IsNullOrEmpty(Role)) {
                  response.StatusCode = 401;
                  response.WriteAsync("You are not Authorised to access this Route");
                  return "";
               }

               return "";
            }
         } catch (Exception e) {
            response.StatusCode = 404;
            response.WriteAsync($"Token is InValid, {e.HResult}");
            return;
         }
      }
   }
}