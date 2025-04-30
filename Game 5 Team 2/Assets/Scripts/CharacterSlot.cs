using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    public CharacterID correctCharacter = CharacterID.None;

    [Header("Colours")]
    public Color correctColour = Color.green;
    public Color wrongColour = Color.red;

    [HideInInspector] public GameObject occupant;
    [HideInInspector] public SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // start fully transparent
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }
    }
}
