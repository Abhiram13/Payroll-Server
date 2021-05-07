using System;
using Npgsql;

namespace Database {
   public delegate ReturnType Reader<ReturnType>(NpgsqlDataReader reader);

   public class Connection {
      private static string connectionString = "Host=localhost;Username=postgres;Password=123;Database=postgres";

      public static Type Sql<Type>(string query, Reader<Type> function) {
         using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
         connection.Open();

         using NpgsqlCommand cmd = new NpgsqlCommand();
         cmd.Connection = connection;

         cmd.CommandText = query;

         return function(cmd.ExecuteReader());
      }
   }
}