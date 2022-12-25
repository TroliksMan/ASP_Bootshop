using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Controllers; 

public class ProductsController : Controller {
    // GET
    public IActionResult Index() {
        return View();
    }
}