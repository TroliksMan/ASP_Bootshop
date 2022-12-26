using BootShopASP.Attributes;
using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Controllers;

[Shared]
public class ProductsController : Controller {
    private MyContext _myContext = new();

    public IActionResult Index() {
        return View();
    }

    public IActionResult Detail(int productID) {
        List<(mProduct, mProductVariant, string)> pr = new();
        var products = _myContext.tbProducts.ToList().OrderBy(x => x.createDate).Take(8);
        foreach (var product in products) {
            pr.Add((product,
                    _myContext.tbProductVariants.ToList().OrderBy(x => x.price).First(x => x.productID == product.id),
                    _myContext.tbImages.First(x => x.productID == product.id && x.isPrimary == true).path)
            );
        }

        this.ViewBag.Products = pr;


        return View();
    }
}