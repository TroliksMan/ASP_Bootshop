using System.Diagnostics;
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

    [HttpGet]
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

        var s = this._myContext.tbProductTypes.ToList();
        this.ViewBag.Products = pr;
        var productModel = this._myContext.tbProducts.Include(x => x.Images).Include(x => x.Category)
            .First(x => x.id == productID);
        var variants =
            this._myContext.tbProductVariants.Where(x => x.productID == productID).OrderBy(x => x.size).Include(x => x.Color);
        this.ViewBag.Variants = variants;
        this.ViewBag.Types = this._myContext.tbProductTypes.Include(x => x.Type).Where(x => x.productID == productID)
            .Select(x => x.Type.name);
        int catID = productModel.Category.id;
        this.ViewBag.Category = this._myContext.tbCategories.Include(x => x.Category).First(x => x.id == catID);

        var variant = variants.FirstOrDefault();
        this.ViewBag.Variant = variant;

        this.ViewBag.sizes = this._myContext.tbProductVariants
            .Where(x => x.productID == productID && x.colorID == variant.colorID).Select(x => x.size)
            .ToList();


        return View(productModel);
    }

    [HttpPost]
    public IActionResult Detail(int productID, int colorID, int size) {
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

        var s = this._myContext.tbProductTypes.ToList();
        this.ViewBag.Products = pr;
        var productModel = this._myContext.tbProducts.Include(x => x.Images).Include(x => x.Category)
            .First(x => x.id == productID);
        var variants =
            this._myContext.tbProductVariants.Where(x => x.productID == productID).OrderBy(x => x.size).Include(x => x.Color);
        this.ViewBag.Variants = variants;
        
        
        this.ViewBag.Types = this._myContext.tbProductTypes.Include(x => x.Type).Where(x => x.productID == productID)
            .Select(x => x.Type.name);
        int catID = productModel.Category.id;
        this.ViewBag.Category = this._myContext.tbCategories.Include(x => x.Category).First(x => x.id == catID);
        var variant = variants.FirstOrDefault(x => x.size == size && x.colorID == colorID);
        if (variant == null) {
            variant = this._myContext.tbProductVariants.Where(x => x.colorID == colorID && x.productID == productID)
                .Include(x => x.Color).FirstOrDefault();
        }

        this.ViewBag.Variant = variant;
        this.ViewBag.sizes = this._myContext.tbProductVariants
            .Where(x => x.productID == productID && x.colorID == variant.colorID).Select(x => x.size)
            .ToList();

        return View(productModel);
    }
}