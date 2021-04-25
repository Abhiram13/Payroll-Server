using System;
using Npgsql;
using System.Text.Json;

namespace Payroll_Server
{
   static class Database
   {
      public static void Connect()
      {
         string cs = "Host=localhost;Username=postgres;Password=123;Database=postgres";

         using NpgsqlConnection connection = new NpgsqlConnection(cs);
         connection.Open();

         using NpgsqlCommand cmd = new NpgsqlCommand();
         cmd.Connection = connection;

         // cmd.CommandText = "INSERT INTO employee_roles(role) VALUES('Admin')";

         cmd.CommandText = "SELECT * FROM employee_roles";
         NpgsqlDataReader reader = cmd.ExecuteReader();
         while (reader.Read())
         {
            var y = JsonSerializer.Serialize(reader[0]);
            Console.WriteLine(y);
         }
      }
   }
}