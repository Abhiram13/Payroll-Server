using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;

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

   public class AuthenticationMiddleware {
      private readonly RequestDelegate _next;

      public AuthenticationMiddleware(RequestDelegate next) {
         _next = next;
      }

      public async Task Invoke(HttpContext context) {
         if (context.Request.Path.StartsWithSegments("/api")) {
            string authHeader = context.Request.Headers["Auth"];
            if (authHeader != null && authHeader.StartsWith("Basic")) {
               //Extract credentials
               string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
               Encoding encoding = Encoding.GetEncoding("iso-8859-1");
               string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

               int seperatorIndex = usernamePassword.IndexOf(':');

               var username = usernamePassword.Substring(0, seperatorIndex);
               var password = usernamePassword.Substring(seperatorIndex + 1);

               if (username == "test" && password == "test") {
                  await _next.Invoke(context);
               } else {
                  context.Response.StatusCode = 401; //Unauthorized
                  return;
               }
            } else {
               // no authorization header
               context.Response.StatusCode = 401; //Unauthorized
               return;
            }
         } else {
            await _next.Invoke(context);
         }             
      }
   }
}