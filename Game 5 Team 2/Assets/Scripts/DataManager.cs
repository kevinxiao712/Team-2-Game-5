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

        // Create a new instances of the data classes and fill them out
        EquipData equipData = new EquipData();
        equipData.item1 = statManager.ManagerItems[0].itemName;
        equipData.item2 = statManager.ManagerItems[1].itemName;
        equipData.item3 = statManager.ManagerItems[2].itemName;
        equipData.guitar = statManager.BandInstruments[0].itemName;
        equipData.bass = statManager.BandInstruments[1].itemName;
        equipData.drums = statManager.BandInstruments[2].itemName;

        ScoreData scoreData = new ScoreData();
        scoreData.preshowScore = scoreManager.preshowScore;
        scoreData.showScore = scoreManager.showScore;
        scoreData.postshowScore = scoreManager.postshowScore;

        // Store the data as JSON
        string equipJson = JsonUtility.ToJson(equipData);
        string scoreJson = JsonUtility.ToJson(scoreData);

        // Write the JSON data to their respective text files
        WriteDataToPath(Application.persistentDataPath + "/equip-data.json", equipJson);
        WriteDataToPath(Application.persistentDataPath + "/score-data.json", scoreJson);
    }

    private void WriteDataToPath(string path, string json)
    {
        if (!File.Exists(path))
            File.WriteAllText(path, json);
        else
            File.AppendAllText(path, json);
    }
}

[System.Serializable]
public class EquipData
{
    public string item1, item2, item3, guitar, bass, drums;
}

[System.Serializable]
public class ScoreData
{
    public int preshowScore, showScore, postshowScore;
}