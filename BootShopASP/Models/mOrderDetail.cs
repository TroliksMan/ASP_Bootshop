using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mOrderDetail {
    [Key] public int id { get; set; }
    [ForeignKey("ProductVariant")] public int productVariantID { get; set; }
    [ForeignKey("Order")] public int orderID { get; set; }
    public int count { get; set; }
    public double price { get; set; }
    public double discount { get; set; }
    public int VAT { get; set; }

    public mProductVariant ProductVariant { get; set; }
    public mOrder Order { get; set; }
}