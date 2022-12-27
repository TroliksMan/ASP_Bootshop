using System.ComponentModel.DataAnnotations;

namespace BootShopASP.Models; 

public class mType {
    [Key] public int id { get; set; }
    public string name { get; set; }
    public List<mProduct> Products { get; set; }
}