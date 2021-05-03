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
}