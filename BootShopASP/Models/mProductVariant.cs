using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Globalization;

namespace BootShopASP.Models;

public class mProductVariant {
    [Key] public int id { get; set; }
    public int productID { get; set; }
    public int colorID { get; set; }

    [RegularExpression("^[0-9]+$", ErrorMessage = "Please enter valid size")]
    [Required(ErrorMessage = "Please enter a size")]
    public int size { get; set; }


    [RegularExpression("^[0-9]+$", ErrorMessage = "Please enter valid stock")]
    [Required(ErrorMessage = "Please enter a stock")]
    public int stock { get; set; }


    [RegularExpression(@"^[0-9]+\.[0-9]{1,2}|[0-9]+,[0-9]{1,2}|[0-9]+$", ErrorMessage = "Please enter valid size")]
    [Required(ErrorMessage = "Please enter a size")]
    public double price { get; set; }

    [RegularExpression(@"^0\.[0-9]{1,2}|0,[0-9]{1,2}|(0|1)$", ErrorMessage = "Valid format is #.## or #,## or #")]
    [Required(ErrorMessage = "Please enter a size")]
    public double discount { get; set; }

    public int VAT { get; set; }

    public mProduct Product { get; set; }
    public mColor Color { get; set; }
}