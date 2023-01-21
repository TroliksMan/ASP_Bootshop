using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Controllers;

public class LoginController : Controller {
    private LoginService _loginService = new();

    [HttpGet]
    public IActionResult Index() {
        return View(new mLogin());
    }

    [HttpPost]
    public IActionResult Index(mLogin model) {
        if (_loginService.Authenticate(model, out string token)) {
            this.HttpContext.Session.SetString("login", token);
            return RedirectToAction("Dashboard", "Admin");
        }

        return View(model);
    }
}