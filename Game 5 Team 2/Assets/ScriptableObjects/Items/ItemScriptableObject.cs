using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public int scoreBoost = 0;
    public bool affectsPreShow = false;
    public bool affectsShow = false;
    public string description;
}
