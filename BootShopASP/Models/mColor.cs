using System.ComponentModel.DataAnnotations;

namespace BootShopASP.Models;

public class mColor {
    [Key] public int id { get; set; }
    public string hexCode { get; set; }
    public string name { get; set; }
}