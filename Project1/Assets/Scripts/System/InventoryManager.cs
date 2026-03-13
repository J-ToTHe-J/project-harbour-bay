using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventorySlot> slots = new List<InventorySlot>();
    public int maxSlots = 20;

    [Header("UI")]
    public GameObject inventoryPanel;
    public Transform gridParent;
    public GameObject slotPrefab;

    // O(1) lookup: which slot index holds this item?
    private Dictionary<ItemData, int> _itemSlotIndex = new Dictionary<ItemData, int>();

    // Object pool — avoids Destroy/Instantiate every refresh
    private List<InventorySlotUI> _pooledSlotUIs = new List<InventorySlotUI>();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
        PrewarmPool(maxSlots);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E))
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    // --- Item logic ---

    public bool AddItem(ItemData item, int amount = 1)
    {
        // O(1) check if we already hold this item
        if (_itemSlotIndex.TryGetValue(item, out int idx))
        {
            InventorySlot slot = slots[idx];
            if (slot.amount < item.maxStack)
            {
                slot.amount = Mathf.Min(slot.amount + amount, item.maxStack);
                RefreshSlotUI(idx);  // refresh only the changed slot
                return true;
            }
            return false; // stack full
        }

        if (slots.Count >= maxSlots) return false;

        slots.Add(new InventorySlot(item, amount));
        _itemSlotIndex[item] = slots.Count - 1;
        RefreshSlotUI(slots.Count - 1);
        return true;
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (!_itemSlotIndex.TryGetValue(item, out int idx)) return false;

        InventorySlot slot = slots[idx];
        slot.amount -= amount;

        if (slot.amount <= 0)
        {
            slots.RemoveAt(idx);
            _itemSlotIndex.Remove(item);
            // Shift all higher indices down by 1
            RebuildIndex();
            RefreshAllUI();
        }
        else
        {
            RefreshSlotUI(idx);
        }
        return true;
    }

    // --- UI ---

    private void PrewarmPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = Instantiate(slotPrefab, gridParent);
            InventorySlotUI ui = go.GetComponent<InventorySlotUI>();
            go.SetActive(false);
            _pooledSlotUIs.Add(ui);
        }
    }

    private void RefreshSlotUI(int idx)
    {
        if (idx < _pooledSlotUIs.Count)
        {
            _pooledSlotUIs[idx].gameObject.SetActive(true);
            _pooledSlotUIs[idx].Setup(slots[idx]);
        }
    }

    public void RefreshAllUI()
    {
        // Enable and update used slots
        for (int i = 0; i < slots.Count; i++)
        {
            _pooledSlotUIs[i].gameObject.SetActive(true);
            _pooledSlotUIs[i].Setup(slots[i]);
        }
        // Hide unused pool slots
        for (int i = slots.Count; i < _pooledSlotUIs.Count; i++)
            _pooledSlotUIs[i].gameObject.SetActive(false);
    }

    private void RebuildIndex()
    {
        _itemSlotIndex.Clear();
        for (int i = 0; i < slots.Count; i++)
            _itemSlotIndex[slots[i].item] = i;
    }
}