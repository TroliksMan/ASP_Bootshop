using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BootShopASP.Models;

public class mProductEdit {
    [Required] public mProduct Product { get; set; }


    [Required(ErrorMessage = "At least one variants has to be created")]
    public List<mProductVariant> ProductVariants { get; set; }

    // public List<(bool, mType)> ProductTypes { get; set; }

    public List<SelectListItem> ProductTypes { get; set; }

    public mProductEdit SetProduct(mProduct product) {
        this.Product = product;
        return this;
    }

    public mProductEdit SetVariants(List<mProductVariant> variants) {
        this.ProductVariants = variants;
        return this;
    }

    // public mProductEdit SetTypes(List<(bool, mType)> types) {
    //     this.ProductTypes = types;
    //     return this;
    // }
    public mProductEdit SetTypes(List<SelectListItem> types) {
        this.ProductTypes = types;
        return this;
    }
}