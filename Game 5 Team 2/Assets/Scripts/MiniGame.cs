using UnityEngine;
using UnityEngine.UI;

public class MinigameBox : MonoBehaviour
{
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

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
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

        if (!isFilling && !isOnCooldown)
        {
            CheckForPlayerInRange(allowedCharacterA);
            CheckForPlayerInRange(allowedCharacterB);
        }

        if (isFilling)
        {
            currentFillTime += Time.deltaTime;
            float progress = currentFillTime / fillDuration;
            if (fillBar != null)
                fillBar.fillAmount = Mathf.Clamp01(progress);

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

        if (character.pressFIndicator != null)
        {
            character.pressFIndicator.SetActive(closeEnough && !isFilling && !isOnCooldown);
        }

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
        }

        if (character.pressFIndicator != null)
        {
            character.pressFIndicator.SetActive(false);
        }

        Debug.Log("Minigame started by " + character.name);
    }

    private void OnFillComplete()
    {
        isFilling = false;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
        }

        float chance = Random.value;
        if (chance < 0.5f)
        {
             ScoreManager.Instance.AddScore(10);
        }
        else
        {
            Debug.Log("Minigame FAILURE for " + currentInteractingCharacter.name);
        }

        StartCooldown();

        currentInteractingCharacter = null;
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
}
