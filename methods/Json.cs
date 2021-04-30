using System;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace System {
   public static class JSON {
      /// <summary>
      /// Converts Http Request body to System.object
      /// </summary>
      /// <typeparam name="DocumentType">Type of system.object that need to convert</typeparam>
      /// <param name="request">HttpRequest that contains Request Body</param>
      /// <returns>Awaitable DocumentType</returns>      
      public static async Task<DocumentType> httpContextDeseriliser<DocumentType>(HttpRequest request) {
         Task<string> str = new StreamReader(request.Body).ReadToEndAsync();
         return JsonSerializer.Deserialize<DocumentType>(await str);
      }

      /// <summary>
      /// Converts JSON string to Object
      /// </summary>
      /// <typeparam name="Document">Type of Object that need to convert</typeparam>
      /// <param name="doc">JSON string</param>
      /// <returns>Document Type</returns>
      public static Document objectDeserializer<Document>(string doc) {
         return JsonSerializer.Deserialize<Document>(doc);
      }

      /// <summary>
      /// Converts given object to JSON String
      /// </summary>
      /// <typeparam name="Document">Type of Document that need to be converted to JSON</typeparam>
      /// <param name="document">Object that need to be converted</param>
      /// <returns>Converted JSON String</returns>
      public static string Serializer<Document>(Document document) {
         return JsonSerializer.Serialize<Document>(document);
      }
   }
}