using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BootShopASP.Models;

public class mCategory {
    [Key] public int id { get; set; }
    public int? parentID { get; set; }
    [MinLength(1)] public string name { get; set; } = "";
    public int leftIndex { get; set; }
    public int rightIndex { get; set; }

    [ForeignKey("parentID")] public mCategory? Category { get; set; }
    public List<mCategory>? Categories { get; set; }

    public List<mProduct> Products { get; set; }
}