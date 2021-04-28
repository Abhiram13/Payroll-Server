using System;
using Npgsql;
using System.Text.Json;
using System.Collections.Generic;

namespace Payroll_Server
{
   static class Roles
   {
      public static string Employee(NpgsqlDataReader reader)
      {
         List<string> list = new List<string>();

         while (reader.Read())
         {
            list.Add((string)reader[0]);
         }

         return JsonSerializer.Serialize(list.ToArray());
      }
   }
}