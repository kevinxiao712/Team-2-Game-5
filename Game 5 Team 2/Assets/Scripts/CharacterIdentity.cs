using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterID { Manager, Guitar, Bass, Drums, Vocals, Dancer}

public class CharacterIdentity : MonoBehaviour
{
    public CharacterID id;   // set in Inspector per prefab
}