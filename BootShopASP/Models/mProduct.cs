using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BootShopASP.Models;

public class mProduct {
    [Key] public int id { get; set; }
    [ForeignKey("Category")] public int categoryID { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string brand { get; set; }
    public string material { get; set; }
    public DateTime createDate { get; set; } = DateTime.Now;

    public mCategory Category { get; set; }
}