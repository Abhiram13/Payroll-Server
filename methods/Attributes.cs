using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using NEmployee;

namespace System {
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class AuthoriseAttribute : ActionFilterAttribute {
		private string[] Roles;
		private HttpResponse response;
		private Employee employee;

		public AuthoriseAttribute(string roles) => Roles = roles.Split(",");

		private void Response(int statuCode, string message) {
			response.StatusCode = statuCode;
			response.WriteAsync(message);
		}

		private bool IsRoleValid() {
			for (int i = 0; i < Roles.Length; i++) {
				if (Roles[i].ToUpper() == employee.designation.ToUpper()) return true;
			}

			return false;
		}

		private bool IsParameterAll() {
			bool isAll = Roles[0].ToUpper() == ("all").ToUpper();
			return isAll ? true : false;
		}

		private AuthoriseResponse isValid(bool doesEmployeeExist) {
			bool valid = true; int status = 200; string message = "";

			if (IsParameterAll()) {
				if (doesEmployeeExist == false) valid = false; status = 404; message = "Employee do not exist";
			} else if (IsRoleValid() == false) valid = false; status = 401; message = "You are not authorised";

			return new AuthoriseResponse() { isvalid = valid, message = message, statusCode = status };
		}

		public override void OnActionExecuted(ActionExecutedContext context) {
			response = context.HttpContext.Response;
			try {
				string id = new Token(context.HttpContext.Request).id;
				bool doesEmployeeExist = EmployeeManagement.IsEmployeeExist(id);
				employee = EmployeeManagement.GetEmployee(id);
				AuthoriseResponse res = isValid(doesEmployeeExist);

				if (!res.isvalid) Response(res.statusCode, res.message);
				return;

			} catch (Exception e) {
				Response(404, $"Please provide Valid Token, {e.Message}");
				return;
			}
		}
	}
}