using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NEmployee;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using Database;

namespace Payroll_Server {
   [Route("api/employee/checkin")]
   public class CheckInController : Controller {
      [HttpGet]
      public int get() {
         DateTime date = DateTime.Now;
         string x = $"{date.Year}/{date.Month}/{date.Day}";
         int che(NpgsqlDataReader reader) => reader.RecordsAffected;
         return Connection.Sql<int>($"INSERT INTO {Table.LOGINS} (id, check_in, date) VALUES ({1}, '{Time.CurrentTime()}', '{x}')", che);
      }
   }
}