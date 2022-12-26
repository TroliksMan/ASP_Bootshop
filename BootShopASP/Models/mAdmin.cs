using System.ComponentModel.DataAnnotations;

namespace BootShopASP.Models;
public class mAdmin {
    [Key] public int id { get; set; }
    public string login { get; set; }
    public string password { get; set; }
}