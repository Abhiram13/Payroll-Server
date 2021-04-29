using System;
using Npgsql;
using System.Text.Json;
using System.Collections.Generic;

namespace Database
{
   public delegate string Reader(NpgsqlDataReader reader);

   public class Connection
   {
      private static string connectionString = "Host=localhost;Username=postgres;Password=123;Database=postgres";      

      public static string Sql(string query, Reader function)
      {         
         using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
         connection.Open();

         using NpgsqlCommand cmd = new NpgsqlCommand();
         cmd.Connection = connection;

         cmd.CommandText = query;

         return function(cmd.ExecuteReader());         
      }
   }
}