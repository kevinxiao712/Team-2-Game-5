using UnityEngine;
using UnityEngine.UI;

public class TimedMinigameBox : MonoBehaviour
{
    [Header("Who Can Interact?")]
    public CharacterController2D allowedCharacterA;
    public CharacterController2D allowedCharacterB;

    [Header("Guaranteed Item (Instant Success)")]
    public ItemScriptableObject guaranteedItem;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.F;

    [Header("Timer Settings")]
    public float timeToDeal = 10f;
    public int successReward = 10;
    public int failPenalty = 10;

    [Header("UI Elements")]
    public Image fillBar; // assign in Inspector (Filled Image)

    [Header("Respawn Settings")]
    public float minRespawnTime = 5f;
    public float maxRespawnTime = 15f;

    // Internal state
    bool isDealTimerRunning = false;
    bool completed = false;
    bool isCooldown = false;
    float dealTimer;
    float cooldownTimer;
    Color originalBarColor;
    CharacterController2D actor;
    SpriteRenderer spriteRenderer;
    Collider2D boxCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<Collider2D>();

        if (fillBar != null)
        {
            originalBarColor = fillBar.color;
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }

        // kick off the first random spawn
        StartCooldown();
    }

    void Update()
    {
        // 1) Cooldown countdown
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                EndCooldown();
            return;
        }

        // 2) If completed, wait for next cooldown
        if (completed) return;

        // 3) Handle "Press F" indicator when timer NOT running
        if (!isDealTimerRunning)
        {
            HandleIndicator(allowedCharacterA);
            HandleIndicator(allowedCharacterB);
        }

        // 4) Countdown the deal timer
        if (isDealTimerRunning)
        {
            dealTimer -= Time.deltaTime;
            if (fillBar != null)
                fillBar.fillAmount = Mathf.Clamp01(dealTimer / timeToDeal);

            // check for early success
            TryInteract(allowedCharacterA);
            TryInteract(allowedCharacterB);

            // time up ¡ú fail
            if (dealTimer <= 0f)
                OnFail();
        }
    }

    void HandleIndicator(CharacterController2D character)
    {
        if (character == null) return;
        bool inRange = character.IsActive
                   && Vector2.Distance(transform.position, character.transform.position) <= interactionRange
                   && !isDealTimerRunning
                   && !isCooldown
                   && !completed;

        character.pressFIndicator?.SetActive(inRange);
    }

    void TryInteract(CharacterController2D character)
    {
        if (!isDealTimerRunning || character == null || !character.IsActive) return;

        if (Vector2.Distance(transform.position, character.transform.position) <= interactionRange
         && Input.GetKeyDown(interactKey))
        {

            actor = character;
            actor.SetActive(false);
            OnSuccess();
        }
    }

    void ActivateBox()
    {
        // show visuals
        spriteRenderer.enabled = true;
        if (boxCollider != null) boxCollider.enabled = true;

        // instant success if item present
        if (PlayerHasGuaranteedItem())
        {
            Debug.Log("Instant minigame success via item!");
            OnSuccess();
            return;
        }

        // otherwise start the timer
        completed = false;
        dealTimer = timeToDeal;
        isDealTimerRunning = true;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 1f;
            fillBar.color = originalBarColor;
        }

        Debug.Log($"Minigame box activated¡ªpress F within {timeToDeal}s");
    }

    void OnSuccess()
    {
        isDealTimerRunning = false;
        completed = true;

        // hide UI
        fillBar?.gameObject.SetActive(false);

        // reward
        ScoreManager.Instance.AddScore(successReward);
        Debug.Log($"Minigame SUCCESS! +{successReward}");

        // unfreeze
        actor?.SetActive(true);

        // start next cooldown
        StartCooldown();
    }

    void OnFail()
    {
        isDealTimerRunning = false;
        completed = true;

        fillBar?.gameObject.SetActive(false);

        ScoreManager.Instance.AddScore(-failPenalty);
        Debug.Log($"Minigame FAIL! ¨C{failPenalty}");

        actor?.SetActive(true);

        StartCooldown();
    }

    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = Random.Range(minRespawnTime, maxRespawnTime);
        isDealTimerRunning = false;

        // hide box
        spriteRenderer.enabled = false;
        if (boxCollider != null) boxCollider.enabled = false;
    }

    void EndCooldown()
    {
        isCooldown = false;
        ActivateBox();
    }

    bool PlayerHasGuaranteedItem()
    {
        if (guaranteedItem == null) return false;
        foreach (var item in StatManager.Instance.ManagerItems)
            if (item == guaranteedItem) return true;
        return false;
    }
}
