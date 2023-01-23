using BootShopASP.Attributes;
using BootShopASP.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Controllers;

[Secured]
public class AdminController : Controller {
    private MyContext _myContext = new();
    const int PAGE_SIZE = 8;

    private IHostEnvironment _iHostEnvironment;

    public AdminController(IHostEnvironment _iHostEnvironment) {
        this._iHostEnvironment = _iHostEnvironment;
    }

    public IActionResult Index() => RedirectToAction("Dashboard");


    public IActionResult LogOut() {
        
        this.HttpContext.Session.Remove("login");
        return RedirectToAction("Index", "Login");
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
    public IActionResult ProductAdd() {
        List<SelectListItem> typesPLZ = new();
        foreach (var type in _myContext.tbTypes.ToList()) {
            typesPLZ.Add(new SelectListItem(type.name, type.id.ToString(), false));
        }

        mProductEdit model = new mProductEdit().SetProduct(new mProduct()).SetVariants(new List<mProductVariant>())
            .SetTypes(typesPLZ);
        model.Product.Images = new();
        model.Product.Images.Add(new() { isPrimary = true });
        model.Product.Images.Add(new());
        model.Product.Images.Add(new());
        model.Product.Images.Add(new());
        return View("ProductEdit", model);
    }

    [ProductEdit]
    public IActionResult ProductEdit(int id) {
        mProduct product = _myContext.tbProducts.Include(x => x.Images).Include(x => x.ProductVariants)
            .Include(x => x.Category).Include(x => x.ProductTypes).ThenInclude(x => x.Type)
            .FirstOrDefault(x => x.id == id);

        if (product == null) {
            return RedirectToAction("Products");
        }


        List<mProductVariant> variants =
            _myContext.tbProductVariants.Include(x => x.Color).Where(x => x.productID == id).ToList();
        var types = _myContext.tbTypes.ToList();
        var productTypes = _myContext.tbProductTypes.Where(x => x.productID == id).Select(x => x.typeID).ToList();
        List<(bool, mType)> typesWTF = new();
        foreach (var type in types) {
            typesWTF.Add((productTypes.Contains(type.id), type));
        }

        List<SelectListItem> typesPLZ = new();
        foreach (var type in types) {
            typesPLZ.Add(new SelectListItem(type.name, type.id.ToString(), (productTypes.Contains(type.id))));
        }

        mProductEdit model = new mProductEdit().SetProduct(product).SetVariants(variants).SetTypes(typesPLZ);
        return View(model);
    }

    public IActionResult Orders(int pagingIndex = 0) {
        int orderCount = _myContext.tbOrders.Count();

        if (pagingIndex < 0)
            pagingIndex = 0;
        else if (pagingIndex > orderCount)
            pagingIndex = 0;

        var orders = _myContext.tbOrders.Include(x => x.Payment).Include(x => x.Delivery).Include(x => x.OrderDetails)
            .ThenInclude(x => x.ProductVariant).ThenInclude(x => x.Product).ThenInclude(x => x.Images).Skip(pagingIndex).Take(PAGE_SIZE);

        this.ViewBag.Orders = orders;

        SetPaging(pagingIndex, orderCount);
        return View();
    }

    public IActionResult Categories() {
        // TODO: Hopefuly simple category editor

        mCategoryEdit categoryEdit = new mCategoryEdit();
        categoryEdit.SetParentCategories(_myContext.tbCategories.Include(x => x.Categories).Include(x => x.Category)
            .Where(x => x.Category == null).ToList());


        return View(categoryEdit);
    }

    [HttpPost]
    public IActionResult Categories(mCategoryEdit cats) {
        foreach (var parent in cats.ParentCategories) {
            if (parent.Categories is null)
                parent.Categories = new();
        }

        foreach (var cat in cats.Categories) {
            cats.ParentCategories[(cat.parentID - 1) ?? 0].Categories.Add(cat);
        }


        foreach (var VARIABLE in cats.ParentCategories) {
            if (VARIABLE.name is null)
                return View("Categories", cats);
        }

        if (!this.ModelState.IsValid) {
            return View("Categories", cats);
        }


        return RedirectToAction("Categories");
    }

    public ActionResult AddVariantRow() => PartialView("ProductEditPartial");

    public IActionResult AddCategoryRow(int id) {
        ViewBag.id = id;
        return PartialView("CategoriesPartial");
    }

    public IActionResult AddParentCatRow(int id) {
        this.ViewBag.id = id;
        return PartialView("CategoriesParentPartial", new mCategory());
    }

    [HttpPost]
    [ProductEdit]
    public IActionResult PostProductEdit(mProductEdit prod, IFormFile formFileFirst, IFormFile formFileSecond,
        IFormFile formFileThird, IFormFile formFileFourth) {
        if (!this.ModelState.IsValid) {
            if (prod.ProductVariants is null)
                prod.ProductVariants = new();

            return View("ProductEdit", prod);
        }

        var product = _myContext.tbProducts.Include(x => x.Images).Include(x => x.ProductVariants)
            .Include(x => x.ProductTypes)
            .FirstOrDefault(x => x.id == prod.Product.id);

        if (prod.Product.id == 0)
            product = new mProduct();
        product.name = prod.Product.name;
        product.description = prod.Product.description;
        product.brand = prod.Product.brand;
        product.ProductVariants = prod.ProductVariants;
        product.categoryID = prod.Product.categoryID;
        product.material = prod.Product.material;
        if (product.id != 0)
            product.ProductTypes.Clear();
        else
            product.ProductTypes = new();
        foreach (var idkplz in prod.ProductTypes) {
            if (idkplz.Selected) {
                product.ProductTypes.Add(new mProductType()
                    { productID = prod.Product.id, typeID = Int32.Parse(idkplz.Value) });
            }
        }

        if (product.id == 0) {
            product.Images = new List<mImage>()
                { new mImage() { isPrimary = true }, new mImage(), new mImage(), new mImage() };
            if (formFileFirst is null || formFileSecond is null || formFileThird is null || formFileFourth is null) {
                return View("ProductEdit", prod);
            }
        }

        if (formFileFirst is not null) {
            mImage img = new();
            var filename = DateTime.Now.ToString("ddMMyyyhhmmss") + formFileFirst.FileName;
            var path = Path.Combine(_iHostEnvironment.ContentRootPath, "wwwroot", "img", "Products", filename);
            var stream = new FileStream(path, FileMode.Create);
            formFileFirst.CopyToAsync(stream);
            img.path = "img/Products/" + filename;
            img.isPrimary = true;
            var index = product.Images.FindIndex(x => x.isPrimary);
            product.Images[index] = img;
        }

        if (formFileSecond is not null) {
            SetImage(formFileSecond, product, 0);
        }

        if (formFileThird is not null) {
            SetImage(formFileThird, product, 1);
        }

        if (formFileFourth is not null) {
            SetImage(formFileFourth, product, 2);
        }

        if (product.id == 0)
            _myContext.tbProducts.Add(product);

        _myContext.SaveChanges();
        return RedirectToAction("Products");
    }

    [HttpGet]
    public IActionResult RemoveProduct(int id) {
        ViewBag.ProductID = id;
        return PartialView("RemoveProductPartial");
    }

    [HttpPost]
    public IActionResult RemoveProductPost(int id) {
        var prod = _myContext.tbProducts.FirstOrDefault(x => x.id == id);
        if (prod != null) {
            this._myContext.tbProducts.Remove(prod);
            this._myContext.SaveChanges();
        }

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

    private void SetImage(IFormFile formFile, mProduct product, int elementIndex) {
        mImage img = new();
        var filename = DateTime.Now.ToString("ddMMyyyhhmmss") + formFile.FileName;
        var path = Path.Combine(_iHostEnvironment.ContentRootPath, "wwwroot", "img", "Products", filename);
        var stream = new FileStream(path, FileMode.Create);
        formFile.CopyToAsync(stream);
        img.path = "img/Products/" + filename;
        img.isPrimary = false;
        var imgChange = product.Images.Where(x => !x.isPrimary).ElementAt(elementIndex);
        var index = product.Images.IndexOf(imgChange);
        product.Images[index] = img;
    }
}