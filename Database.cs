using System;
using Npgsql;

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

         // cmd.CommandText = @"CREATE TABLE cars(id SERIAL PRIMARY KEY, name VARCHAR(255), price INT)";
         // cmd.ExecuteNonQuery();

         cmd.CommandText = "INSERT INTO employee_roles(role) VALUES('Admin')";
         cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Mercedes',57127)";
         // cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Skoda',9000)";
         // cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volvo',29000)";
         // cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Bentley',350000)";
         // cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Citroen',21000)";
         // cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Hummer',41400)";
         // cmd.ExecuteNonQuery();

         // cmd.CommandText = "INSERT INTO cars(name, price) VALUES('Volkswagen',21600)";
         // cmd.ExecuteNonQuery();

         Console.WriteLine("Role Added");
      }
   }
}