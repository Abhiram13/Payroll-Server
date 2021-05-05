using System;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Database;

namespace Payroll_Server {
   [Route("api/employee")]
   public class CheckInController : Controller {
      private static DateTime date = DateTime.Now;
      private string currentDate = $"{date.Year}/{date.Month}/{date.Day}";

      private void updateCheckOut() {
         int func(NpgsqlDataReader reader) => reader.RecordsAffected;
         Connection.Sql<int>(
            $"UPDATE {Table.LOGINS} SET check_out = '{Time.CurrentTime()}' WHERE id = {1} AND date = '{currentDate}' AND check_out IS NULL", 
            func
         );
      }

      [HttpGet]
      [Route("checkin")]
      [Authorise(roles:"all")]
      public string checkIn() {
         int che(NpgsqlDataReader reader) => reader.RecordsAffected;
         int xy = Connection.Sql<int>($"INSERT INTO {Table.LOGINS} (id, check_in, date) VALUES ({1}, '{Time.CurrentTime()}', '{currentDate}')", che);
         return "Checked-In";
      }

      [Route("checkout")]
      public string checkout() {
         updateCheckOut();
         return "Checked-Out";
      }
   }
}