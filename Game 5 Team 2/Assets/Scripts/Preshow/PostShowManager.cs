using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PostShowManager : MonoBehaviour
{
    [Header("Spawn / Prefabs")]
    public Transform spawnPoint;
    public GameObject[] characterPrefabs;   // size 5, order matches enum
    [Header("Slots")]
    public Transform slotParent;
    public CharacterSlot[] slots;
    private int activeIndex = 0; 
    private GameObject currentChar;
    private List<Collider2D> slotColliders = new List<Collider2D>();
    public Canvas resultCanvas;


    void Start()
    {

        foreach (Transform child in slotParent)
        {
            var col = child.GetComponent<Collider2D>();
            if (col != null) slotColliders.Add(col);
        }

        SpawnNextCharacter();
    }

    void Update()
    {
        if (currentChar == null) return;

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Collider2D hitCol = DetectClosestSlot(currentChar.transform.position);
            if (hitCol == null) return;

            CharacterSlot slot = hitCol.GetComponent<CharacterSlot>();
            if (slot == null) return;

            LockCurrentInSlot(slot);   // this already increments and spawns
        }
    }



    Collider2D DetectClosestSlot(Vector2 pos)
    {
        float bestDist = float.MaxValue;
        Collider2D best = null;
        foreach (var col in slotColliders)
        {
            float d = Vector2.Distance(pos, col.transform.position);
            if (d < 0.75f && d < bestDist)  
            {
                bestDist = d;
                best = col;
            }
        }
        return best;
    }

    void LockCurrentInSlot(CharacterSlot slot)
    {
        currentChar.transform.position = slot.transform.position;
        currentChar.GetComponent<CharacterController2D>().SetActive(false);

        slot.occupant = currentChar;           // remember who¡¯s here

        activeIndex++;
        if (activeIndex < characterPrefabs.Length)
            SpawnNextCharacter();
        else
            EvaluatePlacements();              // <= NEW
    }

    void SpawnNextCharacter()
    {
        currentChar = Instantiate(characterPrefabs[activeIndex], spawnPoint.position, Quaternion.identity);
        currentChar.GetComponent<CharacterController2D>().SetActive(true);
        Debug.Log($"Spawned {currentChar.name}");
    }

    void EvaluatePlacements()
    {
        int correct = 0;

        foreach (CharacterSlot slot in slots)
        {
            if (slot.occupant == null) continue;

            CharacterID placed =
                slot.occupant.GetComponent<CharacterIdentity>().id;

            bool isRight = placed == slot.correctCharacter;

            SpriteRenderer sr = slot.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = isRight ? Color.green : Color.red;

            if (isRight) correct++;
        }

        // display result
        if (resultCanvas != null)
        {
            resultCanvas.gameObject.SetActive(true);
            resultCanvas.GetComponentInChildren<TMPro.TMP_Text>().text = $"{correct} / 5 correct!";
        }

        Debug.Log($"Player matched {correct} of 5.");
    }
}
