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
        ctrl.ViewBag.CategoriesBoy = this._myContext.tbCategories.Where(x =>
            x.leftIndex > catChlapecke.leftIndex && x.rightIndex < catChlapecke.rightIndex).Select(x => x.name);
        ctrl.ViewBag.CategoriesGirl = this._myContext.tbCategories.Where(x =>
            x.leftIndex > catDivci.leftIndex && x.rightIndex < catDivci.rightIndex).Select(x => x.name);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}