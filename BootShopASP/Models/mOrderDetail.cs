using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mOrderDetail {
    [Key] public int id { get; set; }
    public int productVariantID { get; set; }
    public int orderID { get; set; }
    public int count { get; set; }
    public double price { get; set; }
    public double discount { get; set; }
    public int VAT { get; set; }

    public mProductVariant ProductVariant { get; set; }
    public mOrder Order { get; set; }
}