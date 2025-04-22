using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstrumentScriptableObject", order = 2)]
public class InstrumentScriptableObject : ItemScriptableObject
{
    public enum InstrumentType
    {
        Guitar,
        Bass,
        Drums
    }

    public InstrumentType instrumentType = InstrumentType.Bass;
    public float minigameLeniency = 1f;
    public float minigameScoreScale = 1f;
}
