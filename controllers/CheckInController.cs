using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NEmployee;
using Microsoft.AspNetCore.Authorization;
using Npgsql;
using Database;
using System.Threading;

namespace Payroll_Server {
   [Route("api/employee/checkin")]
   public class CheckInController : Controller {
      private void timeOut() {
         int che(NpgsqlDataReader reader) => reader.RecordsAffected;
         Connection.Sql<int>($"UPDATE {Table.LOGINS} SET check_out = '{Time.CurrentTime()}' WHERE id = {1}", che);
      }

      private async void thr() {
         Thread thread = new Thread(new ThreadStart(() => timeOut()));
         await Task.Delay(5000);
         thread.Start();
      }

      [HttpGet]
      public string get() {
         DateTime date = DateTime.Now;
         string x = $"{date.Year}/{date.Month}/{date.Day}";
         int che(NpgsqlDataReader reader) => reader.RecordsAffected;
         int xy = Connection.Sql<int>($"INSERT INTO {Table.LOGINS} (id, check_in, date) VALUES ({1}, '{Time.CurrentTime()}', '{x}')", che);

         thr();

         return "OKAY";
      }
   }
}