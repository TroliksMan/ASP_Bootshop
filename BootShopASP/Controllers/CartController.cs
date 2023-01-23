using BootShopASP.Attributes;
using BootShopASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BootShopASP.Controllers;

[Shared]
public class CartController : Controller {
    private MyContext _myContext = new();

    public IActionResult Index() {
        this.ViewBag.Count = 0;
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            sessionCart = new();
            this.HttpContext.Session.SetJson("cart", sessionCart);
            return View();
        }

        List<CartItemVariants> items = new();
        double price = 0;
        List<int> maxStock = new();
        foreach (var item in sessionCart.Items) {
            mProductVariant variant = _myContext.tbProductVariants.Include(x => x.Product).ThenInclude(x => x.Images)
                .Include(x => x.Color).Include(x => x.Product).ThenInclude(x => x.ProductTypes)
                .FirstOrDefault(x => x.id == item.VariantID);
            if (variant is null)
                continue;
            items.Add(new(variant, item.count));
            price += (variant.price * (1 - variant.discount)) * item.count;
            maxStock.Add(variant.stock);
        }

        this.ViewBag.Count = items.Count;
        this.ViewBag.Items = items;
        this.ViewBag.Price = price;
        this.ViewBag.Stock = maxStock;
        return View();
    }

    public IActionResult AddItem(int variantID) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null)
            sessionCart = new mCart();

        sessionCart.AddItem(variantID);
        this.HttpContext.Session.SetJson("cart", sessionCart);
        return RedirectToAction("Index");
    }

    public IActionResult RemoveItem(int variantID) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            sessionCart = new();
            this.HttpContext.Session.SetJson("cart", sessionCart);
            return RedirectToAction("Index");
        }

        if (sessionCart.RemoveItem(variantID))
            this.HttpContext.Session.SetJson("cart", sessionCart);
        return RedirectToAction("Index");
    }

    public IActionResult ChangeCount(int variantID, int count) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        var variant = this._myContext.tbProductVariants.FirstOrDefault(x => x.id == variantID);

        if (sessionCart is null) {
            sessionCart = new();
            this.HttpContext.Session.SetJson("cart", sessionCart);
            return RedirectToAction("Index");
        }

        if (variant is null)
            return RedirectToAction("Index");

        if (count < 1) {
            return RemoveItem(variantID);
        }
        else if (variant.stock >= count) {
            sessionCart.SetAmount(variantID, count);
            this.HttpContext.Session.SetJson("cart", sessionCart);
            // variant.stock -= count;
            // this._myContext.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public IActionResult ClearCart() {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is not null) {
            sessionCart.ClearItems();
            this.HttpContext.Session.SetJson("cart", sessionCart);
        }

        return RedirectToAction("Index");
    }

    public IActionResult CartInfo(mOrder order = null) {
        if (order is null)
            order = new();
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            return RedirectToAction("Index");
        }

        order.OrderDetails = new();
        double price = 0;
        foreach (var item in sessionCart.Items) {
            mProductVariant variant = _myContext.tbProductVariants.Include(x => x.Product).ThenInclude(x => x.Images)
                .FirstOrDefault(x => x.id == item.VariantID);
            if (variant is null)
                continue;
            order.OrderDetails.Add(new mOrderDetail() {
                ProductVariant = variant, Order = order, count = item.count, price = variant.price,
                discount = variant.discount, VAT = variant.VAT
            });
            price += (variant.price * (1 - variant.discount)) * item.count;
        }

        this.ViewBag.Price = price;
        return View(order);
    }

    public IActionResult CartShipping(mOrder order) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            return RedirectToAction("Index");
        }

        order.OrderDetails = new();
        double price = 0;
        foreach (var item in sessionCart.Items) {
            mProductVariant variant = _myContext.tbProductVariants.Include(x => x.Product).ThenInclude(x => x.Images)
                .FirstOrDefault(x => x.id == item.VariantID);
            if (variant is null)
                continue;
            order.OrderDetails.Add(new mOrderDetail() {
                ProductVariant = variant, Order = order, count = item.count, price = variant.price,
                discount = variant.discount, VAT = variant.VAT
            });
            price += (variant.price * (1 - variant.discount)) * item.count;
        }

        this.ViewBag.Shippings = _myContext.tbDeliveries.ToList();
        this.ViewBag.Price = price;
        return View(order);
    }

    public IActionResult CartPayment(mOrder order) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            return RedirectToAction("Index");
        }

        order.OrderDetails = new();
        double price = 0;
        foreach (var item in sessionCart.Items) {
            mProductVariant variant = _myContext.tbProductVariants.Include(x => x.Product).ThenInclude(x => x.Images)
                .FirstOrDefault(x => x.id == item.VariantID);
            if (variant is null)
                continue;
            order.OrderDetails.Add(new mOrderDetail() {
                ProductVariant = variant, Order = order, count = item.count, price = variant.price,
                discount = variant.discount, VAT = variant.VAT
            });
            price += (variant.price * (1 - variant.discount)) * item.count;
        }

        mDelivery delivery = _myContext.tbDeliveries.FirstOrDefault(x => x.id == order.deliveryID);
        if (delivery is null)
            return RedirectToAction("CartShipping", order);
        price += delivery.price;

        this.ViewBag.Delivery = delivery;
        this.ViewBag.Payments = _myContext.tbPayments.ToList();
        this.ViewBag.Price = price;
        return View(order);
    }

    public IActionResult CartFinal(mOrder order) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            return RedirectToAction("Index");
        }

        order.OrderDetails = new();
        double price = 0;
        foreach (var item in sessionCart.Items) {
            mProductVariant variant = _myContext.tbProductVariants.Include(x => x.Product).ThenInclude(x => x.Images)
                .FirstOrDefault(x => x.id == item.VariantID);
            if (variant is null)
                continue;
            order.OrderDetails.Add(new mOrderDetail() {
                ProductVariant = variant, Order = order, count = item.count, price = variant.price,
                discount = variant.discount, VAT = variant.VAT
            });
            price += (variant.price * (1 - variant.discount)) * item.count;
        }

        mDelivery delivery = _myContext.tbDeliveries.FirstOrDefault(x => x.id == order.deliveryID);
        if (delivery is null)
            return RedirectToAction("CartShipping", order);
        price += delivery.price;

        mPayment payment = _myContext.tbPayments.FirstOrDefault(x => x.id == order.paymentID);
        if (payment is null)
            return RedirectToAction("CartShipping", order);
        price += payment.price;

        this.ViewBag.Delivery = delivery;
        this.ViewBag.Payment = payment;
        this.ViewBag.Price = price;
        
        return View(order);
    }

    public IActionResult SendCart(mOrder order) {
        mCart sessionCart = this.HttpContext.Session.GetJson<mCart>("cart");
        if (sessionCart is null) {
            return RedirectToAction("Index");
        }

        order.OrderDetails = new();
        double pric = 0;
        foreach (var item in sessionCart.Items) {
            mProductVariant variant = _myContext.tbProductVariants.Include(x => x.Product).ThenInclude(x => x.Images)
                .FirstOrDefault(x => x.id == item.VariantID);
            if (variant is null)
                continue;
            order.OrderDetails.Add(new mOrderDetail() {
                ProductVariant = variant, Order = order, count = item.count, price = variant.price,
                discount = variant.discount, VAT = variant.VAT
            });
            variant.stock -= item.count;
        }
        
        this._myContext.tbOrders.Add(order);
        this._myContext.SaveChanges();
        this.HttpContext.Session.SetString("cart", "");
        return View();
    }

    public IActionResult FillSession() {
        mCart cart = new();
        cart.AddItem(1);
        cart.AddItem(2);
        cart.AddItem(19);
        cart.SetAmount(2, 5);
        cart.SetAmount(1, 2);

        this.HttpContext.Session.SetJson("cart", cart);

        return RedirectToAction("Index");
    }
}

public struct CartItemVariants {
    public mProductVariant Variant;
    public int Count;

    public CartItemVariants(mProductVariant variant, int count) {
        this.Variant = variant;
        this.Count = count;
    }
}