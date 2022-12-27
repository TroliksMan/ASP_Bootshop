using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mProductVariant {
    [Key] public int id { get; set; }
    public int productID { get; set; }
    public int colorID { get; set; }
    public int size { get; set; }
    public int stock { get; set; }
    public double price { get; set; }
    public double discount { get; set; }
    public int VAT { get; set; }

    public mProduct Product { get; set; }
    public mColor Color { get; set; }
}