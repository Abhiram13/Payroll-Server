namespace Database {
   static class Table {
      public const string ROLES = "roles";
      public const string EMPLOYEE = "employee";
      public const string LOGINS = "logins";
   }

   static class Roles {
      public static readonly string ALL = "All";
      public static readonly string ADMIN = "Admin";
      public static readonly string HR = "HR";
      public static readonly string APP_DEV = "Application Developer";
      public static readonly string SEN_APP_DEV = "Senior Application Developer";
      public static readonly string CEO = "Chief Exceutive Officer";
      public static readonly string CTO = "Chief Technical Officer";
      public static readonly string TL = "Team Leader";
      public static readonly string SL = "Sales Manager";
      public static readonly string PM = "Product Manager";
      public static readonly string TELECALLER = "Tele Caller";
   }
}