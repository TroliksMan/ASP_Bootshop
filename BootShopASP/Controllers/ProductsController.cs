using BootShopASP.Attributes;
using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Controllers;

[Shared]
public class ProductsController : Controller {
    private MyContext _myContext = new();
    const int PAGE_SIZE = 16;

    public IActionResult Index(int pagingIndex = 0, List<int> types = null, int category = -1,
        List<string> colors = null, List<int> sizes = null, string search = null, Order order = Order.nameASC) {
        // -----
        mCategory cat = _myContext.tbCategories.Include(x => x.Category).FirstOrDefault(x => x.id == category);
        this.ViewBag.Category = cat;
        if (cat is not null)
            this.ViewBag.Categories = _myContext.tbCategories.Where(x => x.parentID == cat.id);
        IEnumerable<mProduct> products = null;
        if (cat is null)
            products = _myContext.tbProducts.Include(x => x.ProductVariants).ThenInclude(x => x.Color)
                .Include(x => x.ProductTypes).ThenInclude(x => x.Type).ToList()
                .OrderBy(x => x.createDate).Take(24);
        else
            products = _myContext.tbProducts.Include(x => x.Category).Include(x => x.ProductVariants)
                .ThenInclude(x => x.Color).Include(x => x.ProductTypes).ThenInclude(x => x.Type)
                .Where(x => x.Category.leftIndex >= cat.leftIndex && x.Category.rightIndex <= cat.rightIndex).ToList()
                .OrderBy(x => x.createDate);

        products = FilterProducts(products, search, sizes, colors, types);
        int productCount = products.Count();

        if (pagingIndex < 0)
            pagingIndex = 0;
        else if (pagingIndex > productCount)
            pagingIndex = 0;

        this.ViewBag.Order = order;
        List<(mProduct, mProductVariant, string)> pr = new();

        foreach (var product in products) {
            pr.Add((product,
                    _myContext.tbProductVariants.ToList().OrderBy(x => x.price).First(x => x.productID == product.id),
                    _myContext.tbImages.First(x => x.productID == product.id && x.isPrimary == true).path)
            );
        }

        switch (order) {
            case Order.nameASC:
                pr = pr.OrderBy(x => x.Item1.name).ToList();
                break;
            case Order.nameDESC:
                pr = pr.OrderByDescending(x => x.Item1.name).ToList();
                break;
            case Order.priceASC:
                pr = pr.OrderBy(x => x.Item2.price).ToList();
                break;
            case Order.priceDESC:
                pr = pr.OrderByDescending(x => x.Item2.price).ToList();
                break;
        }

        pr = pr.Skip(pagingIndex).Take(PAGE_SIZE).ToList();
        this.ViewBag.pagingCurrent = pagingIndex;
        this.ViewBag.productCount = productCount;
        if (pagingIndex - PAGE_SIZE < 0)
            this.ViewBag.pagingBack = -1;
        else
            this.ViewBag.pagingBack = pagingIndex - PAGE_SIZE;

        if (pagingIndex + PAGE_SIZE > productCount)
            this.ViewBag.pagingNext = -1;
        else
            this.ViewBag.pagingNext = pagingIndex + PAGE_SIZE;
        this.ViewBag.Products = pr;
        return View();
    }


    [HttpGet]
    public IActionResult Detail(int productID, int variantID = -1) {
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

        this.ViewBag.Products = pr;
        var productModel = this._myContext.tbProducts.Include(x => x.Images).Include(x => x.Category)
            .First(x => x.id == productID);
        var variants =
            this._myContext.tbProductVariants.Where(x => x.productID == productID).OrderBy(x => x.size)
                .Include(x => x.Color);
        this.ViewBag.Variants = variants;
        this.ViewBag.Types = this._myContext.tbProductTypes.Include(x => x.Type).Where(x => x.productID == productID)
            .Select(x => x.Type.name);
        int catID = productModel.Category.id;
        this.ViewBag.Category = this._myContext.tbCategories.Include(x => x.Category).First(x => x.id == catID);


        mProductVariant variant = null;
        if (variantID == -1)
            variant = variants.FirstOrDefault();
        else
            variant = variants.FirstOrDefault(x => x.id == variantID);
        if (variant is null)
            return RedirectToAction("Index", "Home");
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


        var variants =
            this._myContext.tbProductVariants.Where(x => x.productID == productID).OrderBy(x => x.size)
                .Include(x => x.Color);


        var variant = variants.FirstOrDefault(x => x.size == size && x.colorID == colorID);

        if (variant == null)
            variant = variants.Include(x => x.Color)
                .FirstOrDefault(x => x.colorID == colorID && x.productID == productID);

        return RedirectToAction("Detail", "Products", new { productID = productID, variantID = variant.id });
    }

    private IEnumerable<mProduct> FilterProducts(IEnumerable<mProduct> products, string search, List<int> sizes,
        List<string> colors, List<int> types) {
        GetSizes(products);
        GetColors(products);
        GetTypes(products);
        if (search is not null)
            products = products.Where(x => x.name.ToLower().Contains(search.ToLower()));
        this.ViewBag.Search = search;

        if (sizes.Count != 0)
            products = products.Where(x => x.ProductVariants.Any(y => sizes.Contains(y.size)));
        this.ViewBag.SelectedSizes = sizes;

        if (colors.Count != 0)
            products = products.Where(x => x.ProductVariants.Any(y => colors.Contains(y.Color.name)));
        this.ViewBag.SelectedColors = colors;

        if (types.Count != 0)
            products = products.Where(x => x.ProductTypes.Any(y => types.Any(z => z == y.typeID)));
        this.ViewBag.SelectedTypes = types;

        return products;
    }

    private void GetSizes(IEnumerable<mProduct> products) {
        this.ViewBag.Sizes = this._myContext.tbProductVariants.ToList()
            .Where(x => products.Any(y => y.id == x.productID))
            .Select(x => x.size).Distinct().OrderBy(x => x).ToList();
    }

    private void GetColors(IEnumerable<mProduct> products) {
        this.ViewBag.Colors = this._myContext.tbProductVariants.Include(x => x.Color).ToList()
            .Where(x => products.Any(y => y.id == x.productID)).Select(x => x.Color.name).Distinct().OrderBy(x => x)
            .ToList();
    }

    private void GetTypes(IEnumerable<mProduct> products) {
        this.ViewBag.Types = this._myContext.tbProductTypes.Where(x => products.Any(y => y == x.Product))
            .Select(x => x.Type).ToList().DistinctBy(x => x.name);
    }
}

public enum Order {
    nameASC,
    nameDESC,
    priceASC,
    priceDESC,
}