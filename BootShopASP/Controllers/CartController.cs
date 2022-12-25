using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Controllers; 

public class CartController : Controller {
    public IActionResult Cart() {
        return View();
    }
}