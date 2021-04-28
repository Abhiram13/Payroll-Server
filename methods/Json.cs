using System;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Payroll_Server
{
   public static class Json
   {
      /// <summary>
      /// Converts Http Request body to Object
      /// </summary>
      /// <typeparam name="DocumentType">Type of Object that need to convert</typeparam>
      /// <param name="context">HttpContext</param>
      /// <returns>Awaitable Object Type</returns>      
      public static async Task<DocumentType> httpContextDeseriliser<DocumentType>(HttpContext context)
      {
         Task<string> str = new StreamReader(context.Request.Body).ReadToEndAsync();
         return JsonSerializer.Deserialize<DocumentType>(await str);
      }

      /// <summary>
      /// Converts JSON string to Object
      /// </summary>
      /// <typeparam name="Document">Type of Object that need to convert</typeparam>
      /// <param name="doc">JSON string</param>
      /// <returns>Document Type</returns>
      public static Document objectDeserializer<Document>(string doc)
      {
         return JsonSerializer.Deserialize<Document>(doc);
      }

      /// <summary>
      /// Converts given object to JSON String
      /// </summary>
      /// <typeparam name="Document">Type of Document that need to be converted to JSON</typeparam>
      /// <param name="document">Object that need to be converted</param>
      /// <returns>Converted JSON String</returns>
      public static string Serializer<Document>(Document document)
      {
         return JsonSerializer.Serialize<Document>(document);
      }
   }
}