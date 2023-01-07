using System.Reflection;
using BootShopASP.Attributes;
using BootShopASP.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BootShopASP.Controllers;

public class AdminController : Controller {
    private MyContext _myContext = new();
    const int PAGE_SIZE = 8;

    public IActionResult Index() => RedirectToAction("Dashboard");


    public IActionResult Login() {
        // TODO: Authorization
        return View();
    }

    public IActionResult Dashboard() {
        // TODO: Make dashboard, some statistics
        return View();
    }

    public IActionResult Products(int pagingIndex = 0) {
        int productCount = _myContext.tbProducts.Count();


        if (pagingIndex < 0)
            pagingIndex = 0;
        else if (pagingIndex > productCount)
            pagingIndex = 0;


        int count = (productCount - pagingIndex > PAGE_SIZE) ? PAGE_SIZE : productCount - pagingIndex;
        var products = _myContext.tbProducts.Include(x => x.Images).Include(x => x.ProductVariants).Skip(pagingIndex)
            .Take(count);
        this.ViewBag.Products = products;

        SetPaging(pagingIndex, productCount);

        return View();
    }

    [ProductEdit]
    // TODO: Style product edit
    public IActionResult ProductEdit(int id) {
        mProduct product = _myContext.tbProducts.Include(x => x.Images).Include(x => x.ProductVariants)
            .Include(x => x.Category).FirstOrDefault(x => x.id == id);

        if (product == null) {
            return RedirectToAction("Products");
        }


        List<mProductVariant> variants =
            _myContext.tbProductVariants.Include(x => x.Color).Where(x => x.productID == id).ToList();


        mProductEdit model = new mProductEdit().SetProduct(product).SetVariants(variants);
        return View(model);
    }

    public IActionResult Orders(int pagingIndex = 0) {
        // TODO: Make easy order page
        return View();
    }

    public IActionResult Categories(int pagingIndex = 0) {
        // TODO: Hopefuly simple category editor
        return View();
    }

    // TODO: Change name to something more appropriate
    public ActionResult MovieEntryRow() {
        return PartialView("ProductEditPartial");
    }

    [HttpPost]
    [ProductEdit]
    public IActionResult PostProductEdit(mProductEdit prod) {
        if (!this.ModelState.IsValid) {
            if (prod.ProductVariants is null)
                prod.ProductVariants = new();


            return View("ProductEdit", prod);
        }


        if (prod.ProductVariants is null || prod.ProductVariants.Count == 0) {
            return Redirect(Request.Headers["Referer"].ToString());
        }

        var product = _myContext.tbProducts.Include(x => x.Images).Include(x => x.ProductVariants)
            .FirstOrDefault(x => x.id == prod.Product.id);
        if (product == null) {
            return RedirectToAction("Products");
        }

        product.name = prod.Product.name;
        product.description = prod.Product.description;
        product.brand = prod.Product.brand;
        product.ProductVariants = prod.ProductVariants;
        product.categoryID = prod.Product.categoryID;

        _myContext.SaveChanges();
        return RedirectToAction("Products");
    }

    [HttpGet]
    public IActionResult RemoveProduct(int id) {
        ViewBag.ProductID = id;
        return PartialView("RemoveProductPartial");
    }

    // TODO: Finish product removal
    [HttpPost]
    public IActionResult RemoveProductPost(int id) {
        ViewBag.ProductID = id;
        return RedirectToAction("Products");
    }


    private void SetPaging(int pagingIndex, int count) {
        if (pagingIndex - PAGE_SIZE < 0)
            this.ViewBag.pagingBack = -1;
        else
            this.ViewBag.pagingBack = pagingIndex - PAGE_SIZE;

        if (pagingIndex + PAGE_SIZE > count)
            this.ViewBag.pagingNext = -1;
        else
            this.ViewBag.pagingNext = pagingIndex + PAGE_SIZE;
    }
}