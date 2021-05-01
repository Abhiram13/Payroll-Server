using System;
using Microsoft.AspNetCore.Http;

namespace System
{
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
   public class AuthAttribute : Attribute
   {
      public int num;
      public string request;
      public AuthAttribute(int number)
      {
         this.num = number;
         // this.request = req;
      }

      public void check() {
         Console.WriteLine(this.request);
      }
   }   
}