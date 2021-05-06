using System.Threading.Tasks;

namespace System {
   interface IHomeController {
      string Method();
      Task<string> Login();
      Task<string> Signup();
   }

   interface IRoleController {
      string FetchAll();
      Task<string> Add();
   }

   interface ICheckInController {
      string CheckIn();
      string CheckOut();
   }
}