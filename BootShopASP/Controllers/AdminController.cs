using BootShopASP.Attributes;
using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Controllers;

public class AdminController : Controller {
    private MyContext _myContext = new();
    const int PAGE_SIZE = 16;

    public IActionResult Login() {
        return View();
    }

    public IActionResult Dashboard() {
        return View();
    }

    public IActionResult Products(int pagingIndex = 0) {
        int productCount = _myContext.tbProducts.Count();


        if (pagingIndex < 0)
            pagingIndex = 0;
        else if (pagingIndex > productCount)
            pagingIndex = 0;


        int count = (productCount - pagingIndex > PAGE_SIZE) ? PAGE_SIZE : productCount - pagingIndex;
        var products = _myContext.tbProducts.Include(x => x.Images).Skip(pagingIndex).Take(count);
        this.ViewBag.Products = products;

        if (pagingIndex - PAGE_SIZE < 0)
            this.ViewBag.pagingBack = -1;
        else
            this.ViewBag.pagingBack = pagingIndex - PAGE_SIZE;

        if (pagingIndex + PAGE_SIZE > productCount)
            this.ViewBag.pagingNext = -1;
        else
            this.ViewBag.pagingNext = pagingIndex + PAGE_SIZE;


        return View();
    }

    public IActionResult Orders() {
        return View();
    }

    public IActionResult Categories() {
        return View();
    }
}