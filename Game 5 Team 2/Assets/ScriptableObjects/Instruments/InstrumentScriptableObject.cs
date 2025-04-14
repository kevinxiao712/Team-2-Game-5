using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstrumentScriptableObject", order = 2)]
public class InstrumentScriptableObject : ScriptableObject
{
    public enum InstrumentType
    {
        Bass,
        Guitar,
        Drums
    }

    public string instrumentName;
    public InstrumentType instrumentType = InstrumentType.Bass;
    public float minigameLeniency = 1f;
    public float minigameScoreScale = 1f;
}
