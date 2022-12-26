using System.ComponentModel.DataAnnotations;
namespace BootShopASP.Models;


public class mCategory {
    [Key] public int id { get; set; }
    public int? parentID { get; set; }
    public string name { get; set; }
    public int leftIndex { get; set; }
    public int rightIndex { get; set; }

    // public mCategory? Category { get; set; }
}