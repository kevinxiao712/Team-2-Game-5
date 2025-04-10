using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemScriptableObject", order = 1)]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;
    public int managerialSkillsMod = 0;
    public int showPresenceMod = 0;
    public int fanInteractionsMod = 0;
    public bool affectsPreShow = false;
    public bool affectsShow = false;
}
