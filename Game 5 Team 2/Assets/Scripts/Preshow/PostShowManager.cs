using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class PostShowManager : MonoBehaviour
{
    [SerializeField]
    private GameObject postShowCanvas;
    public static PostShowManager Instance { get; private set; }
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
    private List<GameObject> wrongCharacters = new List<GameObject>();
    private int wrongRemaining;
    public GameObject managerPrefab;
    private bool placementPhase = false;
    private bool isPostShowStarted = false;
    private bool isInstructionOpen = false;

    [Header("UI / Instructions")]
    public Canvas instructionsCanvas;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        foreach (Transform child in slotParent)
        {
            var col = child.GetComponent<Collider2D>();
            if (col != null) slotColliders.Add(col);
        }
    }

    public void BeginPostShow()
    {
        Debug.Log("BeginPostShow called");
        gameObject.SetActive(true);


        if (resultCanvas != null)
            resultCanvas.gameObject.SetActive(false);


        wrongCharacters.Clear();
        wrongRemaining = 0;
        activeIndex = 0;
        if (instructionsCanvas != null)
        {
            instructionsCanvas.gameObject.SetActive(true);
            isInstructionOpen = true;
            Time.timeScale = 0f;
            Debug.Log("Instructions shown");
        }

        isPostShowStarted = true;
    }


    void Update()
    {
        if (!isPostShowStarted) return;

        // 1) while instructions are showing, look for Space
        if (isInstructionOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed – closing instructions");
                Time.timeScale = 1f;

                instructionsCanvas.gameObject.SetActive(false);
                isInstructionOpen = false;

                SpawnNextCharacter();      
            }
            return;                   
        }

        // 2) normal post-show handling after instructions are closed
        if (currentChar == null) return;

        if (placementPhase && Input.GetKeyDown(KeyCode.F))
        {
            var hitCol = DetectClosestSlot(currentChar.transform.position);
            if (hitCol == null) return;

            var slot = hitCol.GetComponent<CharacterSlot>();
            if (slot == null) return;

            LockCurrentInSlot(slot);
        }
    }


    public void ResetPostShow()
    {

        if (resultCanvas != null)
            resultCanvas.gameObject.SetActive(false);


        if (currentChar != null)
        {
            Destroy(currentChar);
            currentChar = null;
        }

        foreach (var slot in slots)
        {
            if (slot.occupant != null)
            {
                Destroy(slot.occupant);
                slot.occupant = null;
            }

            var sr = slot.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Color.white;
        }

        wrongCharacters.Clear();
        wrongRemaining = 0;
        activeIndex = 0;

        SpawnNextCharacter();
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
        var rb = currentChar.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;  

        }

        slot.occupant = currentChar;           // remember who’s here

        activeIndex++;
        if (activeIndex < characterPrefabs.Length)
            SpawnNextCharacter();
        else
            EvaluatePlacements();              // <= NEW
    }

    void SpawnNextCharacter()
    {
        if (!placementPhase) placementPhase = true;
        currentChar = Instantiate(characterPrefabs[activeIndex], spawnPoint.position, Quaternion.identity);
        currentChar.GetComponent<CharacterController2D>().SetActive(true);
        Debug.Log($"Spawned {currentChar.name}");
    }

    void EvaluatePlacements()
    {
        int correct = 0;
        wrongCharacters.Clear();

        foreach (CharacterSlot slot in slots)
        {
            if (slot.occupant == null) continue;

            CharacterID placed =
                slot.occupant.GetComponent<CharacterIdentity>().id;

            bool right;


            if (slot.correctCharacter == CharacterID.None)
            {

                right = false;
            }
            else
            {
                right = (placed == slot.correctCharacter);
            }

            SpriteRenderer sr = slot.GetComponent<SpriteRenderer>();
            if (slot.sr != null)
            {
                Color c = right ? slot.correctColour : slot.wrongColour;
                c.a = 1f;                      // fully opaque now
                slot.sr.color = c;
            }

            if (right)
            {
                correct++;
                ScoreManager.Instance.postshowScore += 20; // 20 points per correct drug/twink placement
            }
            else
            {
                wrongCharacters.Add(slot.occupant);

                if (slot.occupant.GetComponent<PickUpable>() == null)
                    slot.occupant.AddComponent<PickUpable>();
            }
        }


        if (resultCanvas != null)
        {
            resultCanvas.gameObject.SetActive(true);
            resultCanvas.GetComponentInChildren<TMPro.TMP_Text>().text =
                $"{correct} / 5 correct!";
        }

        wrongRemaining = wrongCharacters.Count;
        placementPhase = false;
        if (wrongRemaining > 0 && managerPrefab != null)
        {

            if (currentChar != null)
                currentChar.GetComponent<CharacterController2D>().SetActive(false);


            currentChar = Instantiate(managerPrefab,
                                      spawnPoint.position,
                                      Quaternion.identity);

            Rigidbody2D rb = currentChar.GetComponent<Rigidbody2D>();
            if (rb != null) rb.bodyType = RigidbodyType2D.Dynamic;

            currentChar.GetComponent<CharacterController2D>().SetActive(true);
        }
        else
        {
            Debug.Log("Perfect placement");
            EndPostShow();
        }

    }

    public void ReportWrongRemoved()
    {
        wrongRemaining--;
        if (wrongRemaining <= 0)
        {
            Debug.Log("All wrong characters removed");
            EndPostShow();
        }
    }
    private void EndPostShow()
    {
        Debug.Log("No more postshow");
        // TODO: put any cleanup or next‑step logic here
        ScoreManager.Instance.DisplayEndgameCanvas();
        FindAnyObjectByType<PauseMenu>().IncrementPhase();
        postShowCanvas.SetActive(false);
    }
}
