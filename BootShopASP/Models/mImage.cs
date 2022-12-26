using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mImage {
    [Key] public int id { get; set; }
    [ForeignKey("Product")] public int productID { get; set; }
    public string path { get; set; }
    public bool isPrimary { get; set; }
    
    public mProduct Product { get; set; }
}