using System;
using Npgsql;
using System.Text.Json;
using System.Collections.Generic;

namespace Payroll_Server
{
   public delegate string Reader(NpgsqlDataReader reader);

   class Demo
   {
      public long id { get; set; }
      public string demo { get; set; }
   }  

   static class Database
   {
      private static string cs = "Host=localhost;Username=postgres;Password=123;Database=postgres";

      public static string readData(NpgsqlDataReader reader)
      {
         List<Demo> dm = new List<Demo>();

         while (reader.Read())
         {
            dm.Add(new Demo() 
            {
               id = (long)reader[0], demo = (string)reader[1]
            });
         }

         return JsonSerializer.Serialize(dm.ToArray());
      }

      public static string Sql(string query, Reader function)
      {         
         using NpgsqlConnection connection = new NpgsqlConnection(cs);
         connection.Open();

         using NpgsqlCommand cmd = new NpgsqlCommand();
         cmd.Connection = connection;

         cmd.CommandText = query;

         return function(cmd.ExecuteReader());         
      }
   }
}