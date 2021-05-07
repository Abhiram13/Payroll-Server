using System;

namespace System {
   public class Employee {
      public string first_name { get; set; }
      public string last_name { get; set; }
      public string email { get; set; }
      public string designation { get; set; }
      public string user_name { get; set; }
      public string password { get; set; }
      public long supervisor { get; set; }
      public long mobile { get; set; }
      public long id { get; set; }
   }

   public class Login {
      public long id { get; set; }
      public string password { get; set; }
   }

   public class Role {
      public string role { get; set; }
   }

   public class ServerResponse {
      public string Message { get; set; }
      public int Status { get; set; }
   }

   public class AuthoriseResponse {
      public bool isvalid { get; set; }
      public string message { get; set; }
      public int statusCode { get; set; }
   }
}