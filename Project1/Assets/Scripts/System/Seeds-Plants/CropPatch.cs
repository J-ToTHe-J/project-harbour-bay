using UnityEngine;

public class CropPatch : MonoBehaviour
{
    public GameObject plantPrefab;
    private bool hasPlant = false;

    void OnMouseDown()
    {
        if (hasPlant) return;
        if (!SeedInventory.Instance.HasSeed()) return;

        PlantCrop();
    }

    void PlantCrop()
    {
        SeedInventory.Instance.RemoveSeed(1);

        GameObject plant = Instantiate(
            plantPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );

        plant.GetComponent<Plant>().InitializePlant();
        hasPlant = true;
    }

    public void ClearPatch()
    {
        hasPlant = false;
    }
}
