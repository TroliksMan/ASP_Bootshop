using BootShopASP.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Controllers;
[Shared]
public class AdminController : Controller {
    public IActionResult Login() {
        return View();
    }

    public IActionResult Dashboard() {
        return View();
    }
}