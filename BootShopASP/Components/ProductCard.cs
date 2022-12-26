using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace BootShopASP.Components; 

public class ProductCard : ViewComponent {
    public IViewComponentResult Invoke((mProduct, mProductVariant, string) products) {
        this.ViewBag.Product = products.Item1;
        this.ViewBag.ProductVariant = products.Item2;
        this.ViewBag.ImgPath = products.Item3;
    
        return View();
    }       
}