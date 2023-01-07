using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mProduct {
    [Key] public int id { get; set; }
    public int categoryID { get; set; }
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
    [MaxLength(100, ErrorMessage = "Name must be less than 100 characters long")]
    public string name { get; set; }
    [MinLength(3, ErrorMessage = "Description must be at least 3 characters long")]
    public string description { get; set; }
    [MinLength(3, ErrorMessage = "Brand name must be at least 3 characters long")]
    public string brand { get; set; }
    [MinLength(3, ErrorMessage = "Material must be at least 3 characters long")]
    public string material { get; set; }
    public DateTime createDate { get; set; } = DateTime.Now;

    public mCategory Category { get; set; }
    public List<mImage> Images { get; set; }
    public List<mType> Types { get; set; }
    public List<mProductVariant> ProductVariants { get; set; }
}