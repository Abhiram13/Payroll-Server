using System;

namespace System {
   public static class OBJECT {
      public static string GetKeys<ObjectType>(ObjectType obj) {
         string keys = "";
         foreach (var key in typeof(ObjectType).GetProperties()) {
            keys += $"{key.Name}, ";
         }

         return keys.Substring(0, keys.Length - 2);
      }

      public static string GetValues<ObjectType>(ObjectType obj) {
         string values = "";
         foreach (var value in typeof(ObjectType).GetProperties()) {
            values += (value.PropertyType == typeof(string)) ? $"'{value.GetValue(obj)}', " : $"{value.GetValue(obj)}, ";
         }

         return values.Substring(0, values.Length - 2);
      }
   }

   public static class Token {
      public static string Encode(string str) {
         var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(str);
         return System.Convert.ToBase64String(plainTextBytes);
      }

      public static string Decode(string str) {
         var base64EncodedBytes = System.Convert.FromBase64String(str);
         return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
      }
   }
}