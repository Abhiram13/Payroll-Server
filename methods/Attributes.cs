using System;

namespace System
{
   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
   public class AuthAttribute : Attribute
   {
      public int num;
      public AuthAttribute(int number)
      {
         this.num = number;
      }
   }
}