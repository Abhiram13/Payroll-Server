using System;
using Npgsql;
using System.Text.Json;

namespace Payroll_Server
{
   static class Database
   {
      private static string cs = "Host=localhost;Username=postgres;Password=123;Database=postgres";

      public static void Sql(string query)
      {
         using NpgsqlConnection connection = new NpgsqlConnection(cs);
         connection.Open();

         using NpgsqlCommand cmd = new NpgsqlCommand();
         cmd.Connection = connection;

         cmd.CommandText = query;
         NpgsqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            var y = JsonSerializer.Serialize(reader[0]);
            Console.WriteLine(y);
         }
      }
   }
}