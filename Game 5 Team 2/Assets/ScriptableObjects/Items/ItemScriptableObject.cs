using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    [TextArea]
    public string description;
    [TextArea]
    public string unlockFlavorText;
}
