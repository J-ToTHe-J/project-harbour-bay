using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    public int currentDay = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDay();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextDay()
    {
        currentDay++;
        SaveDay();
        Debug.Log("Day: " + currentDay);
    }

    void SaveDay()
    {
        PlayerPrefs.SetInt("CurrentDay", currentDay);
        PlayerPrefs.Save();
    }

    void LoadDay()
    {
        currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
    }
}

