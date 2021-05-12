using System;
using Microsoft.AspNetCore.Http;
using Database;
using Npgsql;
using System.Threading.Tasks;

namespace Payroll_Server {
   class LoginManager {
      // verify whether given id exists
      // if yes, then verify whether password if correct
      // if yes, then create a token and authorise
      private static Login user;

      private static bool VerifyUserId() {
         string QUERY = $"SELECT id FROM {Table.EMPLOYEE} WHERE id = {user.id}";
         return Connection.Sql<bool>(QUERY, (reader) => reader.HasRows);
      }

      private static bool VerifyPassword() {
         string QUERY = $"SELECT id FROM {Table.EMPLOYEE} WHERE id = {user.id} AND password = {user.password}";
         return Connection.Sql<bool>(QUERY, (reader) => reader.HasRows);
      }

      private static void UnAuthoriseUser() {

      }

      public async static Task<string> Login(HttpRequest requestBody) {
         user = await JSON.httpContextDeseriliser<Login>(requestBody);

         if (VerifyUserId() && VerifyPassword()) {
            return StringValue.Encode($"{user.id}: {user.password}");
         }

         return "";
      }
   }
}