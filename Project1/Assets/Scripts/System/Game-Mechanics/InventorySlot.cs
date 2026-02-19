[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;

    public InventorySlot(ItemData newItem, int newAmount)
    {
        item = newItem;
        amount = newAmount;
    }
}
