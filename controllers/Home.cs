using System;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using NEmployee;
using Database;
using Npgsql;
using System.Collections.Generic;

namespace Payroll_Server
{
   [Route("")]
   public class HomeController : Controller
   {
      [HttpGet]
      [HttpPost]
      [HttpPut]
      [HttpDelete]
      public async Task<string> method()
      {
         using (StreamReader reader = new StreamReader(Request.Body))
         {
            string body = await reader.ReadToEndAsync();
            Console.WriteLine(body);
         }     
         return "Welcome to Payroll";
      }

      [HttpPost]
      [Route("login")]
      public void login() { }

      [HttpPost]
      [Route("signup")]
      public async Task<string> signup()
      {
         Employee emp;
         using (StreamReader reader = new StreamReader(Request.Body))
         {
            string body = await reader.ReadToEndAsync();
            emp= JSON.objectDeserializer<Employee>(body);
         }

         int employeeCount = Connection.Sql<int>($"SELECT id FROM {Table.EMPLOYEE}", check);

         if (employeeCount > 0)
         {
            return "Employee already Existed";
         }

         string values = $"'{emp.first_name}', '{emp.last_name}', '{emp.designation}', '{emp.email}', {emp.mobile}, {emp.id}, {emp.supervisor}, '{emp.password}', '{emp.user_name}'";
         string keys = "first_name, last_name, designation, email, mobile, id, supervisor, password, user_name";
         string query = $"INSERT INTO {Table.EMPLOYEE}({keys}) VALUES({values})";

         return Connection.Sql<string>(query, func);
      }

      private int check(Npgsql.NpgsqlDataReader reader)
      {
         List<long> list = new List<long>();

         while (reader.Read())
         {
            list.Add((long)reader[0]);
         }

         return list.Count;
      }

      private string func(Npgsql.NpgsqlDataReader reader)
      {
         if (reader.RecordsAffected == 1)
         {
            return "Successfully Added";
         }

         return "Not Added";
      }
   }
}