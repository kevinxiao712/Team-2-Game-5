using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class CoOpMinigameBox : MonoBehaviour
{
    [Header("Required Characters")]
    public CharacterController2D characterA;
    public CharacterController2D characterB;

    [Header("Interaction")]
    public float interactionRange = 2f;
    public KeyCode interactKey = KeyCode.F;
    public float fillDuration = 15f;          // seconds bar fills

    [Header("UI")]
    public Image fillBar;                         // Filled Image

    bool isFilling = false;
    float fillTimer = 0f;

    void Start()
    {
        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }
    }

    void Update()
    {
        if (isFilling)
        {
            if (!BothInRange()) CancelFilling();
            else FillingTick();
        }
        else TryStart();
    }

    bool BothInRange()
    {
        if (characterA == null || characterB == null) return false;

        return Vector2.Distance(transform.position, characterA.transform.position) <= interactionRange
            && Vector2.Distance(transform.position, characterB.transform.position) <= interactionRange;
    }

    void TryStart()
    {
        if (!BothInRange()) return;

        // Only the active player may press F to begin
        CharacterController2D active =
            characterA.IsActive ? characterA :
            characterB.IsActive ? characterB : null;

        if (active == null) return;
        if (!Input.GetKeyDown(interactKey)) return;

        /* freeze both characters */
        characterA.FreezeForTask();
        characterB.FreezeForTask();

        /* begin filling */
        isFilling = true;
        fillTimer = 0f;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 0f;
        }
    }

    void FillingTick()
    {
        fillTimer += Time.deltaTime;
        if (fillBar != null)
            fillBar.fillAmount = Mathf.Clamp01(fillTimer / fillDuration);

        if (fillTimer >= fillDuration)
            Complete();
    }

    void CancelFilling()
    {
        isFilling = false;
        fillTimer = 0f;
        fillBar?.gameObject.SetActive(false);

        characterA.Unfreeze();
        characterB.Unfreeze();

        Debug.Log("Co-op minigame cancelled ¨C one player stepped away.");
    }

    void Complete()
    {
        isFilling = false;
        fillBar?.gameObject.SetActive(false);

        characterA.Unfreeze();
        characterB.Unfreeze();

        ScoreManager.Instance.AddScore(10);      // always succeed
        gameObject.SetActive(false);             // one-shot
    }
}
