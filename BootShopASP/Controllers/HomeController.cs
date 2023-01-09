using BootShopASP.Attributes;
using Microsoft.AspNetCore.Mvc;
using BootShopASP.Models;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Controllers;

[Shared]
public class HomeController : Controller {
    private MyContext _myContext = new();

    public IActionResult Index() {
        // AddImages();
        // AddVariantsStart();
        // AddColors();
        // AddVariants();
        // AddProductTypes();
        // this.ViewBag.Categories = _myContext.tbCategories.Where(x => x.parentID == null);
        // this.ViewBag.Chlapecke = _myContext.tbCategories.Where(x => x.leftIndex > 1 && x.rightIndex < 18);
        // this.ViewBag.Divci = _myContext.tbCategories.Where(x => x.leftIndex > 19 && x.rightIndex < 38);
        // this.ViewBag.Doplnky = _myContext.tbCategories.Where(x => x.leftIndex > 39 && x.rightIndex < 42);
        // this.ViewBag.Ostatni = _myContext.tbCategories.Where(x => x.leftIndex > 43 && x.rightIndex < 48);

        var c = _myContext.tbProductTypes.ToList();

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

    private void AddImages() {
        for (int i = 1; i < 20; i++) {
            this._myContext.tbImages.Add(new mImage() {
                isPrimary = false,
                productID = i,
                path = "img/Products/bota4.png"
            });
            this._myContext.tbImages.Add(new mImage() {
                isPrimary = false,
                productID = i,
                path = "img/Products/bota5.png"
            });
        }

        this._myContext.SaveChanges();
    }

    private void AddProducts() {
        this._myContext.tbProducts.Add(new mProduct() {
            categoryID = 15,
            name = "Velcro Navy",
            description =
                "Tyhle botky mají styl! Kožené barefoot tenisky vhodné na užší až normální nožku. Vyrobené z kvalitní hladké kůže v kombinaci s textilem. Tenká podešev přesahuje přes prsty, aby byla botka chráněna před poškozením. Dva suché zipy na rychlé a samostatné obouvání.",
            brand = "Bundgaard Benjamin",
            material = "kůže",
        });
        this._myContext.tbProducts.Add(new mProduct() {
            categoryID = 15,
            name = "Cosmos Flower",
            description =
                "Tyhle botky mají styl! Kožené barefoot tenisky vhodné na užší až normální nožku. Vyrobené z kvalitní hladké kůže v kombinaci s textilem. Tenká podešev přesahuje přes prsty, aby byla botka chráněna před poškozením. Dva suché zipy na rychlé a samostatné obouvání.",
            brand = "Bundgaard Benjamin",
            material = "kůže",
        });
        this._myContext.tbProducts.Add(new mProduct() {
            categoryID = 15,
            name = "Velcro Dark Grey",
            description =
                "Tyhle botky mají styl! Kožené barefoot tenisky vhodné na užší až normální nožku. Vyrobené z kvalitní hladké kůže v kombinaci s textilem. Tenká podešev přesahuje přes prsty, aby byla botka chráněna před poškozením. Dva suché zipy na rychlé a samostatné obouvání.",
            brand = "Bundgaard Benjamin",
            material = "kůže",
        });
        this._myContext.tbProducts.Add(new mProduct() {
            categoryID = 15,
            name = "High Dino",
            description =
                "Tyhle botky mají styl! Kožené barefoot tenisky vhodné na užší až normální nožku. Vyrobené z kvalitní hladké kůže v kombinaci s textilem. Tenká podešev přesahuje přes prsty, aby byla botka chráněna před poškozením. Dva suché zipy na rychlé a samostatné obouvání.",
            brand = "Bundgaard Benjamin",
            material = "kůže",
        });
        this._myContext.tbProducts.Add(new mProduct() {
            categoryID = 15,
            name = "Mint Flavour",
            description =
                "Tyhle botky mají styl! Kožené barefoot tenisky vhodné na užší až normální nožku. Vyrobené z kvalitní hladké kůže v kombinaci s textilem. Tenká podešev přesahuje přes prsty, aby byla botka chráněna před poškozením. Dva suché zipy na rychlé a samostatné obouvání.",
            brand = "Bundgaard Benjamin",
            material = "kůže",
        });

        this._myContext.SaveChanges();
    }

    private void AddVariantsStart() {
        for (int i = 1; i < 20; i++) {
            this._myContext.tbProductVariants.Add(new mProductVariant() {
                productID = i,
                colorID = 3,
                size = 10,
                stock = 153,
                price = 999,
                discount = 0,
                VAT = 15
            });
        }

        this._myContext.SaveChanges();
    }

    private void AddVariants() {
        this._myContext.tbProductVariants.Add(new mProductVariant() {
            productID = 1,
            colorID = 1,
            size = 42,
            stock = 50,
            price = 150,
            discount = 0.3,
            VAT = 21
        });
        this._myContext.tbProductVariants.Add(new mProductVariant() {
            productID = 1,
            colorID = 2,
            size = 50,
            stock = 20,
            price = 120,
            discount = 0.8,
            VAT = 21
        });
        this._myContext.tbProductVariants.Add(new mProductVariant() {
            productID = 5,
            colorID = 1,
            size = 42,
            stock = 30,
            price = 20,
            discount = 0.3,
            VAT = 21
        });
        this._myContext.tbProductVariants.Add(new mProductVariant() {
            productID = 11,
            colorID = 1,
            size = 42,
            stock = 50,
            price = 10,
            discount = 0.3,
            VAT = 21
        });
        this._myContext.tbProductVariants.Add(new mProductVariant() {
            productID = 20,
            colorID = 1,
            size = 42,
            stock = 50,
            price = 2200,
            discount = 0.3,
            VAT = 21
        });

        this._myContext.SaveChanges();
    }

    private void AddColors() {
        this._myContext.tbColors.Add(new mColor() {
            hexCode = "#d92b2b",
            name = "RED"
        });
        this._myContext.tbColors.Add(new mColor() {
            hexCode = "##32a852",
            name = "GREEN"
        });
        this._myContext.tbColors.Add(new mColor() {
            hexCode = "#312bd9",
            name = "BLUE"
        });
        this._myContext.tbColors.Add(new mColor() {
            hexCode = "#312bd9",
            name = "PINK"
        });

        this._myContext.SaveChanges();
    }

    private void AddProductTypes() {
        foreach (mProduct myContextTbProduct in this._myContext.tbProducts) {
            this._myContext.tbProductTypes.Add(new mProductType() {
                productID = myContextTbProduct.id,
                typeID = 1
            });
            this._myContext.tbProductTypes.Add(new mProductType() {
                productID = myContextTbProduct.id,
                typeID = 2
            });
            this._myContext.tbProductTypes.Add(new mProductType() {
                productID = myContextTbProduct.id,
                typeID = 3
            });
        }

        this._myContext.SaveChanges();
    }
}