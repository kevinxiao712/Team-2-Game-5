using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterID {None, Manager, Guitar, Bass, Drums, Vocals, Dancer}

public class CharacterIdentity : MonoBehaviour
{
    public CharacterID id;   // set in Inspector per prefab
}