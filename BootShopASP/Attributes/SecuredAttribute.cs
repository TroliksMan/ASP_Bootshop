using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BootShopASP.Attributes;

public class SecuredAttribute : Attribute, IAuthorizationFilter {
    private LoginService _loginService = new();

    public void OnAuthorization(AuthorizationFilterContext context) {
        string? token = context.HttpContext.Session.GetString("login");

        if (token == null || !_loginService.VerifyToken(token)) {
            context.Result = new RedirectToActionResult("Index", "Login", new { });
        }
    }
}