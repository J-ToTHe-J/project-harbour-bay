using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    public void Setup(InventorySlot slot)
    {
        icon.sprite = slot.item.icon;
        icon.enabled = slot.item.icon != null;
        amountText.text = slot.amount > 1 ? slot.amount.ToString() : "";
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = "";
    }
}