using UnityEngine;
using UnityEngine.UI;

public class TimedMinigameBox : MonoBehaviour
{
    [Header("Which Character(s) Can Interact?")]
    public CharacterController2D allowedCharacterA;
    public CharacterController2D allowedCharacterB;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.F;
    public float fillDuration = 3f; // How long the slider takes to fill

    [Header("Failure Timer Settings")]
    public float timeToDeal = 10f;  // Total time the player has to handle this minigame
    public int failPenalty = 10;    // Score penalty if the player fails
    public int successReward = 10;  // Score reward if the player succeeds

    [Header("Cooldown Settings")]
    public float cooldownDuration = 5f; // How long the box stays disabled after success/fail
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    [Header("UI Elements")]
    public Image fillBar; // The fill bar UI (type = Filled Image)

    private float dealTimer;
    private bool isDealTimerRunning = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        // Get the SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the fill bar is hidden or reset at the start
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }

        StartCooldown();
    }

    private void Update()
    {
        // 1) Cooldown handling
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                EndCooldown();
            }
        }

        // 2) If we have an active timer, count down
        if (isDealTimerRunning && !isOnCooldown)
        {
            dealTimer -= Time.deltaTime;
            // Update the fill bar to show time left
            if (fillBar != null)
            {
                float fillAmount = dealTimer / timeToDeal;  // Goes from 1 down to 0
                fillBar.fillAmount = Mathf.Clamp01(fillAmount);
            }

            // If time reaches zero => fail
            if (dealTimer <= 0f)
            {
                OnFail();
            }
        }

        // 3) Check if a player is in range & can press F to succeed
        if (!isOnCooldown && isDealTimerRunning)
        {
            CheckForPlayerInRange(allowedCharacterA);
            CheckForPlayerInRange(allowedCharacterB);
        }
    }

    private void CheckForPlayerInRange(CharacterController2D character)
    {
        if (character == null) return;

        // Calculate distance
        float distance = Vector2.Distance(transform.position, character.transform.position);
        bool closeEnough = distance <= interactionRange;

        // Toggle the player's "PressFIndicator"
        if (character.pressFIndicator != null)
        {
            // Show if close, timer is running, not on cooldown
            character.pressFIndicator.SetActive(closeEnough && !isOnCooldown && isDealTimerRunning);
        }

        // If close and presses F => success
        if (closeEnough && Input.GetKeyDown(interactKey))
        {
            OnSuccess();
        }
    }
    private void OnSuccess()
    {
        // Stop the timer
        isDealTimerRunning = false;

        // Give the reward
        ScoreManager.Instance.AddScore(successReward);

        // Hide the fill bar
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
        }

        Debug.Log("Minigame SUCCESS! Awarded " + successReward + " points.");

        // Start cooldown
        StartCooldown();
    }

    private void OnFail()
    {
        // Timer ran out => fail
        isDealTimerRunning = false;

        // Subtract penalty
        ScoreManager.Instance.AddScore(-failPenalty);

        // Hide the fill bar
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
        }

        Debug.Log("Minigame FAIL: Timed out. Subtracted " + failPenalty + " points.");

        // Start cooldown
        StartCooldown();
    }
    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;

        // Hide the box entirely
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void EndCooldown()
    {
        isOnCooldown = false;
        ActivateBox();
    }

    private void ActivateBox()
    {
        // Show the sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        // Reset the timer
        dealTimer = timeToDeal;
        isDealTimerRunning = true;

        // Show/initialize the fill bar
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 1f; // full at start
        }

        Debug.Log("Box is active again! You have " + timeToDeal + "s to press F.");
    }
}
