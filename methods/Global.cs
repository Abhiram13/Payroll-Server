using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Npgsql;
using Database;
using Microsoft.Extensions.Primitives;

namespace System {
   public static class OBJECT {
      public static string GetKeys<ObjectType>(ObjectType obj) {
         string keys = "";
         foreach (var key in typeof(ObjectType).GetProperties()) {
            keys += $"{key.Name}, ";
         }

         return keys.Substring(0, keys.Length - 2);
      }

      public static string GetValues<ObjectType>(ObjectType obj) {
         string values = "";
         foreach (var value in typeof(ObjectType).GetProperties()) {
            values += (value.PropertyType == typeof(string)) ? $"'{value.GetValue(obj)}', " : $"{value.GetValue(obj)}, ";
         }

         return values.Substring(0, values.Length - 2);
      }
   }

   public static class StringValue {
      public static string Encode(string str) {
         byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
         return System.Convert.ToBase64String(plainTextBytes);
      }

      public static string Decode(string str) {
         byte[] base64EncodedBytes = System.Convert.FromBase64String(str);
         return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
      }
   }

   public static class Token {

      /// <summary>
      /// Verifies given Token and authorises Employee based on Roles given
      /// </summary>
      /// <param name="roles">Shall be Authorised only to the given Roles</param>
      /// <param name="header">Request Headers that contains Token value</param>
      /// <returns>ServerResponse contains Message and StatusCode</returns>
      public static ServerResponse Verify(string[] roles, IHeaderDictionary header) {
         StringValues token = header["Auth"];
         if (token == "") {
            return new ServerResponse() {
               Message = "Please provide valid Token",
               Status = 404,
            };
         }

         string[] decode;

         try {
            decode = StringValue.Decode(token).Split(":");
            return Connection.Sql<ServerResponse>($"SELECT designation FROM {Table.EMPLOYEE} WHERE id = {decode[0]}", VerifyUser);

            ServerResponse VerifyUser(Npgsql.NpgsqlDataReader reader) {
               string role = "";
               while (reader.Read()) role = (string)reader[0];

               string Role = Array.Find(roles, r => r.ToUpper() == role.ToUpper());

               if (Role == null || string.IsNullOrEmpty(Role)) {
                  return new ServerResponse() {
                     Message = "You are not Authorised to access this Route",
                     Status = 401,
                  };
               }

               return new ServerResponse() {
                  Message = "Authorise",
                  Status = 200,
               };
            }
         } catch (Exception e) {
            return new ServerResponse() {
               Message = $"Token is InValid, {e.HResult}",
               Status = 404,
            };
         }
      }
   }

   public class AutAttribute : ActionFilterAttribute {
      private string[] Roles;

      public AutAttribute(string roles) {
         this.Roles = roles.Split(",");
      }

      public override void OnActionExecuted(ActionExecutedContext context) {
         Console.WriteLine(this.Roles[0]);
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
            var x = Connection.Sql($"SELECT designation FROM {Table.EMPLOYEE} WHERE id = {decode[0]}", VerifyUser);

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