using BootShopASP.Attributes;
using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Controllers;

[Shared]
public class ProductsController : Controller {
    private MyContext _myContext = new();

    public IActionResult Index() {
        return View();
    }

    public IActionResult Detail(int productID) {
        if (productID == 0)
            return RedirectToAction("Index", "Home");


        List<(mProduct, mProductVariant, string)> pr = new();
        var products = _myContext.tbProducts.ToList().OrderBy(x => x.createDate).Take(8);
        foreach (var product in products) {
            pr.Add((product,
                    _myContext.tbProductVariants.ToList().OrderBy(x => x.price).First(x => x.productID == product.id),
                    _myContext.tbImages.First(x => x.productID == product.id && x.isPrimary == true).path)
            );
        }

        var s  = this._myContext.tbProductTypes.ToList();
        this.ViewBag.Products = pr;

        this.ViewBag.Product = this._myContext.tbProducts.Include(x => x.Images).Include(x => x.Category).First(x => x.id == productID);
        this.ViewBag.Variants = this._myContext.tbProductVariants.Where(x => x.productID == productID).Include(x => x.Color);
        this.ViewBag.Types = this._myContext.tbProductTypes.Include(x => x.Type).Where(x => x.productID == productID)
            .Select(x => x.Type.name);
        int catID = this.ViewBag.Product.Category.id;
        this.ViewBag.Category = this._myContext.tbCategories.Include(x=> x.Category).First(x => x.id == catID);
        return View();
    }

    public IActionResult FuckYou() {
        this.ViewBag.Product = this._myContext.tbProducts.Include(x => x.Images).Include(x => x.Category).First(x => x.id == 2);
        
        return Content(null);
    }
}