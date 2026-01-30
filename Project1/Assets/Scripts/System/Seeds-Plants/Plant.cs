using UnityEngine;

public class Plant : MonoBehaviour
{
    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;

    private SpriteRenderer sr;
    private int plantedDay;
    private SaveController saveController;
    private bool fullyGrown;

    private CropPatch parentPatch;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        saveController = FindObjectOfType<SaveController>();
        parentPatch = transform.parent.GetComponent<CropPatch>();
    }

    public void InitializePlant()
    {
        plantedDay = saveController.currentDay;
        sr.sprite = stage1;
    }

    void Update()
    {
        int daysPassed = saveController.currentDay - plantedDay;

        if (daysPassed == 1)
            sr.sprite = stage2;

        if (daysPassed >= 2 && !fullyGrown)
        {
            sr.sprite = stage3;
            fullyGrown = true;
        }

        if (fullyGrown && daysPassed >= 4)
        {
            SeedInventory.Instance.AddSeed(1);
            Cleanup();
        }
    }

    void OnMouseDown()
    {
        if (!fullyGrown) return;
        Harvest();
    }

    void Harvest()
    {
        SeedInventory.Instance.AddSeed(1);

        if (Random.Range(1, 69421) == 69420)
            SeedInventory.Instance.AddSeed(1);

        Cleanup();
    }

    void Cleanup()
    {
        parentPatch.ClearPatch();
        Destroy(gameObject);
    }
}
