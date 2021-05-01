using System;
using Microsoft.AspNetCore.Http;
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

   public class ServerResponse {
      public string Message { get; set; }
      public int Status { get; set; }
   }
}