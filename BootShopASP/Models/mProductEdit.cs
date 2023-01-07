using System.ComponentModel.DataAnnotations;

namespace BootShopASP.Models;

public class mProductEdit {
    [Required]
    public mProduct Product { get; set; }
    [Required(ErrorMessage = "At least one variants has to be created")]
    public List<mProductVariant> ProductVariants { get; set; }

    public mProductEdit SetProduct(mProduct product) {
        this.Product = product;
        return this;
    }

    public mProductEdit SetVariants(List<mProductVariant> variants) {
        this.ProductVariants = variants;
        return this;
    }
}