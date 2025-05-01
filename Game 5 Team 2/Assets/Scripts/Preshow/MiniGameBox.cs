using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinigameBox : MonoBehaviour
{
    [Header("Item for Instant Completion")]
    public ItemScriptableObject guaranteedItem;

    [Header("Who Can Interact?")]
    public CharacterController2D allowedCharacterA;
    public CharacterController2D allowedCharacterB;

    [Header("Interaction")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.F;
    public float fillDuration = 3f;

    [Header("UI")]
    public Image fillBar;

    // Internal
    bool isFilling = false;
    bool completed = false;
    bool swapped = false;           
    float fillTime = 0f;
    float targetTime = 0f;
    Color originalBar;
    CharacterController2D actor;

    [Header("Optional proximity object")]
    public GameObject proximityObject;

    void Start()
    {
        if (fillBar != null)
        {
            originalBar = fillBar.color;
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }

        if (proximityObject != null)
            proximityObject.SetActive(false);
    }

    void Update()
    {


        if (completed) return;


        UpdatePressFIndicator(allowedCharacterA);
        UpdatePressFIndicator(allowedCharacterB);
        ShowOrHideProximityObject();
        // Only check the one active character
        if (!isFilling)
        {
            if (allowedCharacterA != null && allowedCharacterA.IsActive)
                TryBeginFill(allowedCharacterA);

            if (allowedCharacterB != null && allowedCharacterB.IsActive)
                TryBeginFill(allowedCharacterB);
        }

        // If we¡¯re filling, advance the bar
        if (isFilling)
        {
            fillTime += Time.deltaTime;
            if (fillBar != null)
                fillBar.fillAmount = Mathf.Clamp01(fillTime / targetTime);

            if (fillTime >= targetTime)
                CompleteMinigame();
        }
    }
    void ShowOrHideProximityObject()
    {
        if (proximityObject == null) return;

        CharacterController2D active = null;

        if (allowedCharacterA != null && allowedCharacterA.IsActive)
            active = allowedCharacterA;
        else if (allowedCharacterB != null && allowedCharacterB.IsActive)
            active = allowedCharacterB;

        bool show = false;

        if (active != null && PlayerHasGuaranteedItem())
        {
            show = Vector2.Distance(transform.position, active.transform.position)
                   <= interactionRange;
        }

        if (proximityObject.activeSelf != show)
            proximityObject.SetActive(show);
    }

    void TryBeginFill(CharacterController2D character)
    {

        if (Vector2.Distance(transform.position, character.transform.position) > interactionRange) return;
        if (!Input.GetKeyDown(interactKey)) return;

        actor = character;
        swapped = false;
        isFilling = true;
        fillTime = 0f;

        bool hasItem = PlayerHasGuaranteedItem();
        targetTime = hasItem ? 0f : fillDuration;

        // show bar
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 0f;
            fillBar.color = originalBar;
        }
        actor.FreezeForTask();




        if (!hasItem)                        // only for timed path
        {
            CharacterController2D other =
                (actor == allowedCharacterA) ? allowedCharacterB : allowedCharacterA;

            // the GameManager guard will ignore the call if ¡®other¡¯ is busy
            if (other != null)
            {
                FindObjectOfType<GameManager>()?.SwitchActiveCharacter();
                swapped = true;              // remember that we tried
            }
        }

        if (targetTime <= 0f)
            CompleteMinigame();
    }

    void UpdatePressFIndicator(CharacterController2D chr)
    {
        if (chr == null) return;

        bool inside = Vector2.Distance(transform.position, chr.transform.position)
                      <= interactionRange
                      && !isFilling                    // don¡¯t show while bar filling
                      && !completed;                   // or after done

        if (inside) chr.AddInRange();
        else chr.RemoveInRange();
    }


    void CompleteMinigame()
    {
        actor.Unfreeze();
        isFilling = false;
        completed = true;
        fillBar?.gameObject.SetActive(false);

        actor.Unfreeze(!swapped);       

        ScoreManager.Instance.AddScore(10);   // or ¨C10 on fail
        gameObject.SetActive(false);          // one-shot
    }

    bool PlayerHasGuaranteedItem()
    {
        if (guaranteedItem == null) return false;
        foreach (var item in StatManager.Instance.ManagerItems)
            if (item == guaranteedItem) return true;
        return false;
    }
}
