using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Controllers;

public class AdminController : Controller {
    public IActionResult Login() {
        return View();
    }

    public IActionResult Dashboard() {
        return View();
    }
}