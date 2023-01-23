namespace BootShopASP.Models;

public class mCart {
    private List<CartItem> _items = new();
    public List<CartItem> Items => _items;

    public void AddItem(int variantID) {
        if (!_items.Any(x => x.VariantID == variantID))
            _items.Add(new CartItem(variantID, 1));
    }

    public void SetAmount(int variantID, int count) {
        var item = _items.FirstOrDefault(x => x.VariantID == variantID);
        if (item.Equals(default(CartItem)))
            return;
        item.count = count;
    }

    public bool RemoveItem(int variantID) {
        var item = _items.FirstOrDefault(x => x.VariantID == variantID);
        if (item is null)
            return false;

        return _items.Remove(item);
    }

    public void ClearItems() {
        _items.Clear();
    }
}

public class CartItem {
    public int VariantID;
    public int count;

    public CartItem(int variant, int count) {
        VariantID = variant;
        this.count = count;
    }
}