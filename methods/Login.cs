using System;
using Microsoft.AspNetCore.Http;
using Database;
using Npgsql;
using System.Threading.Tasks;

namespace Payroll_Server {
	static class LoginManager {
		private static Login user;

		private static bool VerifyUserId() {
			string QUERY = $"SELECT id FROM {Table.EMPLOYEE} WHERE id = {user.id}";
			return Connection.Sql<bool>(QUERY, (reader) => reader.HasRows);
		}

		private static bool VerifyPassword() {
			string QUERY = $"SELECT id FROM {Table.EMPLOYEE} WHERE id = {user.id} AND password = '{user.password}'";
			return Connection.Sql<bool>(QUERY, (reader) => reader.HasRows);
		}

		private static string UnAuthoriseUser() {
			if (!VerifyPassword()) {
				return "Employee password do not match";
			}

			return "Employee does not Exist";
		}

		public async static Task<string> Login(HttpRequest requestBody) {
			user = await JSON.httpContextDeseriliser<Login>(requestBody);

			if (VerifyUserId() && VerifyPassword()) {
				return StringValue.Encode($"{user.id}: {user.password}");
			}

			return UnAuthoriseUser();
		}
	}

	// class SignInManager {
	//    // check if required values are provided
	//    // notify user when required fields are empty or wrong
	//    // if all are okay, then create user
	//    private static Employee employee;

	//    public static async void SignIn(HttpRequest request) {
	//       employee = await JSON.httpContextDeseriliser<Employee>(request);
	//    }
	// }
}