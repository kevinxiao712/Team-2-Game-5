using UnityEngine;
using UnityEngine.UI;

public class MinigameBox : MonoBehaviour
{
    [Header("Which Character(s) Can Interact")]
    public CharacterController2D allowedCharacterA;   
    public CharacterController2D allowedCharacterB; 

    [Header("Interaction Settings")]
    public float interactionRange = 2f;  
    public KeyCode interactKey = KeyCode.F;
    public float fillDuration = 3f;     

    [Header("UI Elements")]
    public Image fillBar;             

    private bool isFilling = false;
    private float currentFillTime = 0f;

    private CharacterController2D currentInteractingCharacter = null;

    private void Start()
    {
        // Hide the fill bar at start
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }
    }
    void Update()
    {
        if (!isFilling)
        {
            CheckForInteraction();
        }
        else
        {
            currentFillTime += Time.deltaTime;
            fillBar.fillAmount = Mathf.Clamp01(currentFillTime / fillDuration);

            if (currentFillTime >= fillDuration)
            {
                OnFillComplete();
            }
        }
    }

    private void CheckForInteraction()
    {
        if (allowedCharacterA != null && IsCharacterCloseEnough(allowedCharacterA))
        {
            // If within range, wait for input
            if (Input.GetKeyDown(interactKey))
            {
                StartFilling(allowedCharacterA);
            }
        }

        if (allowedCharacterB != null && IsCharacterCloseEnough(allowedCharacterB))
        {
            if (Input.GetKeyDown(interactKey))
            {
                StartFilling(allowedCharacterB);
            }
        }
    }

    private bool IsCharacterCloseEnough(CharacterController2D character)
    {
        float distance = Vector2.Distance(transform.position, character.transform.position);
        return distance <= interactionRange;
    }

    private void StartFilling(CharacterController2D character)
    {
        currentInteractingCharacter = character;

        isFilling = true;
        currentFillTime = 0f;
        fillBar.fillAmount = 0f;
        fillBar.gameObject.SetActive(true);


    }

    private void OnFillComplete()
    {
        isFilling = false;
        fillBar.gameObject.SetActive(false);

        float chance = Random.value; // 0.0 to 1.0
        if (chance < 0.5f)
        {
            ScoreManager.Instance.AddScore(10);
        }
        else
        {
            Debug.Log("Minigame Failure for: " + currentInteractingCharacter.name);
        }



        currentInteractingCharacter = null;
    }
}
