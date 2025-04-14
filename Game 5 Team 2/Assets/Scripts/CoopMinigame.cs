using UnityEngine;
using UnityEngine.UI;

public class CoOpMinigameBox : MonoBehaviour
{
    [Header("Required Characters")]
    public CharacterController2D characterA;
    public CharacterController2D characterB;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;    
    public KeyCode interactKey = KeyCode.F;
    public float fillDuration = 3f;       

    [Header("UI Elements")]
    public Image fillBar;                

    private bool isFilling = false;
    private float currentFillTime = 0f;

    private void Start()
    {
        // Hide the fill bar at start
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }
    }

    private void Update()
    {
        if (!isFilling)
        {
            // Only start if both are in range and we press F
            if (BothPlayersInRange() && Input.GetKeyDown(interactKey))
            {
                StartFilling();
            }
        }
        else
        {
            if (!BothPlayersInRange())
            {
                CancelFilling();
                return;
            }

            // Update fill time
            currentFillTime += Time.deltaTime;
            float progress = currentFillTime / fillDuration;
            fillBar.fillAmount = Mathf.Clamp01(progress);

            // If filled up completely, success
            if (progress >= 1f)
            {
                OnFillComplete();
            }
        }
    }

    private bool BothPlayersInRange()
    {
        if (characterA == null || characterB == null)
            return false;

        float distA = Vector2.Distance(transform.position, characterA.transform.position);
        float distB = Vector2.Distance(transform.position, characterB.transform.position);

        return distA <= interactionRange && distB <= interactionRange;
    }

    private void StartFilling()
    {
        isFilling = true;
        currentFillTime = 0f;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 0f;
        }

        Debug.Log("Remain in Range");
    }

    private void CancelFilling()
    {
        isFilling = false;
        currentFillTime = 0f;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
        }

        Debug.Log("Out of Range");
    }

    private void OnFillComplete()
    {
        isFilling = false;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
        }

        // Simple random success/failure example
        float chance = Random.value; // 0.0 to 1.0
        if (chance < 0.5f)
        {
            ScoreManager.Instance.AddScore(10);
        }
        else
        {
            Debug.Log("FAILURE");
        }
    }
}
