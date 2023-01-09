namespace BootShopASP.Models; 

public class mCategoryEdit {
    public List<mCategory> ParentCategories { get; set; }
    public List<mCategory> Categories { get; set; } = new();
    public mCategoryEdit SetParentCategories(List<mCategory> cats) {
        this.ParentCategories = cats;
        return this;
    }

}