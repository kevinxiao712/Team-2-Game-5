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

    void Start()
    {
        if (fillBar != null)
        {
            originalBar = fillBar.color;
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }
    }

    void Update()
    {
        if (completed) return;

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

    void TryBeginFill(CharacterController2D character)
    {
        // must be close and press F
        if (Vector2.Distance(transform.position, character.transform.position) > interactionRange) return;
        if (!Input.GetKeyDown(interactKey)) return;

        actor = character;             
        actor.SetActive(false);
        isFilling = true;
        fillTime = 0f;
        swapped = false;

        bool hasItem = PlayerHasGuaranteedItem();
        targetTime = hasItem ? 0f : fillDuration;

        // UI
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 0f;
            fillBar.color = originalBar;
        }

        if (!hasItem)
        {
            CharacterController2D other = (actor == allowedCharacterA) ? allowedCharacterB : allowedCharacterA;

            bool otherBusy = other == null || !other.IsActive || other.GetComponent<CharacterController2D>().IsFrozen;
            if (!otherBusy)
            {
                FindObjectOfType<GameManager>()?.SwitchActiveCharacter();
                swapped = true;
            }
            foreach (MinigameBox box in FindObjectsOfType<MinigameBox>())
            {
                if (box != this && box.isFilling && box.actor == other)
                {
                    otherBusy = true;
                    break;
                }
            }

            if (!otherBusy && other != null)
            {
                FindObjectOfType<GameManager>()?.SwitchActiveCharacter();
                swapped = true;                      // remember we swapped
            }
        }

        // now freeze the actor
        actor.SetActive(false);

        // instant-win path
        if (targetTime <= 0f)
            CompleteMinigame();
    }



    void CompleteMinigame()
    {
        isFilling = false;
        completed = true;

        // hide bar
        if (fillBar != null)
            fillBar.gameObject.SetActive(false);

        // only re-enable the actor if we did NOT swap
        if (!swapped && actor != null)
            actor.SetActive(true);

        ScoreManager.Instance.AddScore(10);

        // disable this box forever
        gameObject.SetActive(false);
    }

    bool PlayerHasGuaranteedItem()
    {
        if (guaranteedItem == null) return false;
        foreach (var item in StatManager.Instance.ManagerItems)
            if (item == guaranteedItem) return true;
        return false;
    }
}
