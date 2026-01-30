using UnityEngine;
using TMPro;

public class SeedInventory : MonoBehaviour
{
    public static SeedInventory Instance;

    public int seedCount = 5;
    public TextMeshProUGUI seedText;

    void Awake()
    {
        Instance = this;
        UpdateUI();
    }

    public bool HasSeed()
    {
        return seedCount > 0;
    }

    public void RemoveSeed(int amount = 1)
    {
        seedCount -= amount;
        UpdateUI();
    }

    public void AddSeed(int amount = 1)
    {
        seedCount += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        seedText.text = $"Seeds: {seedCount}";
    }
}
