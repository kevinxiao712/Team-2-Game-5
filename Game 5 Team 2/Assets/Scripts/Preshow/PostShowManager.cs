using UnityEngine;
using System.Collections.Generic;

public class PostShowManager : MonoBehaviour
{
    [Header("Spawn / Prefabs")]
    public Transform spawnPoint;
    public GameObject[] characterPrefabs;   // size 5, order matches enum
    [Header("Slots")]
    public Transform slotParent;     

    private int activeIndex = 0; 
    private GameObject currentChar;
    private List<Collider2D> slotColliders = new List<Collider2D>();

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
            Collider2D slot = DetectClosestSlot(currentChar.transform.position);
            if (slot != null)
            {
                LockCurrentInSlot(slot);
                activeIndex++;

                if (activeIndex < characterPrefabs.Length)
                    SpawnNextCharacter();
                else
                    Debug.Log("All 5 placed.");
            }
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

    void LockCurrentInSlot(Collider2D slot)
    {
        currentChar.transform.position = slot.transform.position;
        currentChar.GetComponent<CharacterController2D>().SetActive(false);
    }

    void SpawnNextCharacter()
    {
        currentChar = Instantiate(characterPrefabs[activeIndex], spawnPoint.position, Quaternion.identity);
        currentChar.GetComponent<CharacterController2D>().SetActive(true);
        Debug.Log($"Spawned {currentChar.name}");
    }
}
