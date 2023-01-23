using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Attributes;

public class SharedAttribute : Attribute, IActionFilter {
    private MyContext _myContext = new();

    public void OnActionExecuting(ActionExecutingContext context) {
        var ctrl = context.Controller as Controller;

        mCategory catChlapecke = this._myContext.tbCategories.First(x => x.name == "Chlapecké");
        mCategory catDivci = this._myContext.tbCategories.First(x => x.name == "Dívčí");

        mCart sessionCart = ctrl.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is not null)
            ctrl.ViewBag.ProductCountHeader = sessionCart.Items.Count;
        else
            ctrl.ViewBag.ProductCountHeader = 0;

        ctrl.ViewBag.CategoriesBoy = this._myContext.tbCategories.Where(x =>
            x.leftIndex > catChlapecke.leftIndex && x.rightIndex < catChlapecke.rightIndex);
        ctrl.ViewBag.CategoriesGirl = this._myContext.tbCategories.Where(x =>
            x.leftIndex > catDivci.leftIndex && x.rightIndex < catDivci.rightIndex);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}