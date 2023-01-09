using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models; 
public class mProductType {
    [Key] public int id { get; set; }
    [ForeignKey("Product")]
    public int productID { get; set; }
    [ForeignKey("Type")]
    public int typeID { get; set; }

    public mType Type { get; set; }
    public mProduct Product { get; set; }
}