using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Attributes;

public class ProductEditAttribute : Attribute, IActionFilter {
    private MyContext _myContext = new();
    public void OnActionExecuting(ActionExecutingContext context) {
        var cats = _myContext.tbCategories.Include(x => x.Category).ToList();
        var ctrl = context.Controller as Controller;
        
        ctrl.ViewBag.Categories = cats;
        ctrl.ViewBag.MainCats = cats.Where(x => x.parentID == null);
        ctrl.ViewBag.Types = _myContext.tbTypes.ToList();
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}