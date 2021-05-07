using System;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Database;

namespace Payroll_Server {
   [Route("api/employee")]
   public class CheckInController : Controller, ICheckInController {
      private static DateTime date = DateTime.Now;
      private string currentDate = $"{date.Year}/{date.Month}/{date.Day}";

      private int func(NpgsqlDataReader reader) => reader.RecordsAffected;

      private void UpdateCheckOut() {
         Connection.Sql<int>(
            $"UPDATE {Table.LOGINS} SET check_out = '{Time.CurrentTime()}' WHERE id = {new Token(Request).id} AND date = '{currentDate}' AND check_out IS NULL", 
            func
         );
      }

      [HttpGet]
      [Route("checkin")]
      [Authorise(roles:"all")]
      public string CheckIn() {
         int xy = Connection.Sql<int>(
            $"INSERT INTO {Table.LOGINS} (id, check_in, date) VALUES ({new Token(Request).id}, '{Time.CurrentTime()}', '{currentDate}')",
            func
         );

         return "Checked-In";
      }

      [Route("checkout")]
      public string CheckOut() {
         UpdateCheckOut();
         return "Checked-Out";
      }
   }
}