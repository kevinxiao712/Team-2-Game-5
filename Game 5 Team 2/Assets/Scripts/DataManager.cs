using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public void WriteToJson()
    {
        // Get references to the score and stat managers
        ScoreManager scoreManager = FindAnyObjectByType<ScoreManager>();
        StatManager statManager = FindAnyObjectByType<StatManager>();

        // Create a new TrackedData class and fill it out
        TrackedData data = new TrackedData();
        data.item1 = statManager.ManagerItems[0].itemName;
        data.item2 = statManager.ManagerItems[1].itemName;
        data.item3 = statManager.ManagerItems[2].itemName;
        data.guitar = statManager.BandInstruments[0].itemName;
        data.bass = statManager.BandInstruments[1].itemName;
        data.drums = statManager.BandInstruments[2].itemName;
        data.preshowScore = scoreManager.preshowScore;
        data.showScore = scoreManager.showScore;
        data.postshowScore = scoreManager.postshowScore;

        // Store the data as JSON
        string json = JsonUtility.ToJson(data);

        // Write the JSON data to a text file
        string path = Application.persistentDataPath + "/data.json";
        if (!File.Exists(path))
            File.WriteAllText(path, json);
        else
            File.AppendAllText(path, json);
    }
}

[System.Serializable]
public class TrackedData
{
    public string item1, item2, item3, guitar, bass, drums;
    public int preshowScore, showScore, postshowScore;
}