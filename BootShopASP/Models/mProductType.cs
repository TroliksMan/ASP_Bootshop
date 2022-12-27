using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models; 
public class mProductType {
    [Key] public int id { get; set; }
    public int productID { get; set; }
    public int typeID { get; set; }

    public mType Type { get; set; }
    public mProduct Product { get; set; }
}