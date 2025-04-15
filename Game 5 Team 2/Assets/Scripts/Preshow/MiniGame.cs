using UnityEngine;
using UnityEngine.UI;

public class MinigameBox : MonoBehaviour
{

    public ItemScriptableObject guaranteedItem;


    [Header("Which Character(s) Can Interact?")]
    public CharacterController2D allowedCharacterA;
    public CharacterController2D allowedCharacterB;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.F;
    public float fillDuration = 3f;

    [Header("Cooldown Settings")]
    public float cooldownDuration = 5f;   // How long it stays disabled after success/fail
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    [Header("UI Elements")]
    public Image fillBar; 

    // Internal state
    private bool isFilling = false;
    private float currentFillTime = 0f;
    private CharacterController2D currentInteractingCharacter = null;
    private bool wasInRangeLastFrame = false;

    private Color originalFillBarColor;

    private SpriteRenderer spriteRenderer;



    private void Start()
    {
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
            originalFillBarColor = fillBar.color;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                EndCooldown();
            }
        }


        if (allowedCharacterA == null) return;

        float distance = Vector2.Distance(transform.position, allowedCharacterA.transform.position);
        bool closeEnough = distance <= interactionRange;

        // Range logic for "AddInRange" / "RemoveInRange"
        if (closeEnough && !wasInRangeLastFrame)
        {
            allowedCharacterA.AddInRange();
        }
        else if (!closeEnough && wasInRangeLastFrame)
        {
            allowedCharacterA.RemoveInRange();
        }
        wasInRangeLastFrame = closeEnough;

        // If not on cooldown and not filling, check if we can start filling
        if (!isOnCooldown && !isFilling)
        {
            CheckForPlayerInRange(allowedCharacterA);
        }

        // If filling, update bar
        if (isFilling)
        {
            currentFillTime += Time.deltaTime;
            float progress = currentFillTime / fillDuration;

            if (fillBar != null)
                fillBar.fillAmount = Mathf.Clamp01(progress);

            // Fill complete
            if (progress >= 1f)
            {
                OnFillComplete();
            }
        }
    }

    private void CheckForPlayerInRange(CharacterController2D character)
    {
        if (character == null) return;

        float distance = Vector2.Distance(transform.position, character.transform.position);
        bool closeEnough = distance <= interactionRange;

        if (closeEnough && Input.GetKeyDown(interactKey))
        {
            StartFilling(character);
        }
    }

    private void StartFilling(CharacterController2D character)
    {
        isFilling = true;
        currentFillTime = 0f;
        currentInteractingCharacter = character;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 0f;
            fillBar.color = originalFillBarColor;  // Ensure it's the normal color
        }

        // Hide "F" indicator
        if (character.pressFIndicator != null)
        {
            character.pressFIndicator.SetActive(false);
        }

        Debug.Log("Minigame started by " + character.name);
    }

    private void OnFillComplete()
    {
        isFilling = false;

        // Decide success or fail
        float chance = Random.value;
        bool success = (chance < 0.5f);

        if (PlayerHasGuaranteedItem())
        {
            Debug.Log("Auto-Success! Player has the guaranteed item for this minigame.");
            // e.g., add points or do success logic
            ScoreManager.Instance.AddScore(10);
        }
        else
        {
            if (success)
            {
                // success
                ScoreManager.Instance.AddScore(10);
            }
            else
            {
                Debug.Log("Minigame FAILURE");
            }
        }

        // Start a coroutine to flash the fill bar color 
        StartCoroutine(FlashFillBar(success));

        currentInteractingCharacter = null;
    }

    private System.Collections.IEnumerator FlashFillBar(bool success)
    {
        if (fillBar != null)
        {
            // Choose green if success, red if fail
            fillBar.color = success ? Color.green : Color.red;
        }

        // Keep the fill bar visible for 1 second
        yield return new WaitForSeconds(1f);

        // Revert fill bar color
        if (fillBar != null)
        {
            fillBar.color = originalFillBarColor;
            // Now hide the bar entirely
            fillBar.gameObject.SetActive(false);
        }

        // Then go to cooldown
        StartCooldown();
    }

    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.5f;
            spriteRenderer.color = c;
        }
    }

    private void EndCooldown()
    {
        isOnCooldown = false;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }

        Debug.Log("Minigame is active again.");
    }

    private bool PlayerHasGuaranteedItem()
    {
        var items = StatManager.Instance.ManagerItems;
        if (guaranteedItem == null) return false;
        foreach (var item in items)
        {
            if (item == guaranteedItem)
            {
                return true;
            }
        }
        return false;
    }
}
