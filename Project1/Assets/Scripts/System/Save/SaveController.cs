using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    public int currentDay = 1;

    [Header("UI References")]
    public GameObject savePromptUI; // The Y/N Panel
    public CanvasGroup fadeScreen;  // A black UI Image with a CanvasGroup

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");
        savePromptUI.SetActive(false);
    }

    // Called by the "Yes" Button in your UI
    public void OnClickYes()
    {
        savePromptUI.SetActive(false);
        StartCoroutine(ProgressDaySequence());

        Time.timeScale = 1f;
    }

    // Called by the "No" Button
    public void OnClickNo()
    {
        savePromptUI.SetActive(false);
        // Unlock player movement here if you locked it

        Time.timeScale = 1f;
    }

    private IEnumerator ProgressDaySequence()
    {
        // 1. Fade to Black
        yield return StartCoroutine(Fade(1));

        // 2. Logic: Increment Day and Save
        currentDay++;
        SaveGame();

        yield return new WaitForSeconds(1f); // Brief pause while black

        // 3. Fade back in
        yield return StartCoroutine(Fade(0));
    }

    public void SaveGame()
    {
        SaveData Savedata = new SaveData
        {
            playerPosition = GameObject.FindWithTag("Player").transform.position,
            currentDay = currentDay
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(Savedata));
        Debug.Log("Game Saved. Day: " + currentDay);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float speed = 2f;
        while (!Mathf.Approximately(fadeScreen.alpha, targetAlpha))
        {
            fadeScreen.alpha = Mathf.MoveTowards(fadeScreen.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
    }
}