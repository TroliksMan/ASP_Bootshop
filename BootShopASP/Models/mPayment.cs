using System.ComponentModel.DataAnnotations;

namespace BootShopASP.Models; 

public class mPayment {
    [Key] public int id { get; set; }
    public string name { get; set; }
    public int price { get; set; }
}