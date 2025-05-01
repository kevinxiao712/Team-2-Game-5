using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public GameObject pressFIndicator;
    public bool isActive;
    private int boxesInRangeCount = 0;
    public bool IsFrozen => !isActive;
    public Sprite activeSprite;
    public Sprite inactiveSprite;
    public bool IsActive { get { return isActive; } }

    public bool IsBusy { get; private set; }
    SpriteRenderer sr;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        if (sr == null)
        {
            Debug.LogError($"[{name}] No SpriteRenderer found on this GameObject or its children.");
        }
        else if (inactiveSprite != null)
        {
            sr.sprite = inactiveSprite;
        }

        if (pressFIndicator != null)
            pressFIndicator.SetActive(isActive && boxesInRangeCount > 0);
    }


    public void SetActive(bool active)
    {
        isActive = active;

        if (sr != null)
            sr.sprite = active && activeSprite != null ? activeSprite: inactiveSprite;

        if (pressFIndicator != null)
            pressFIndicator.SetActive(false);
    }

    void Update()
    {
        if (!isActive)
        {
            // If not active, stop velocity
            rb.velocity = Vector2.zero;
            return;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(moveX, moveY) * moveSpeed;
    }
    public void AddInRange() { boxesInRangeCount++; }
    public void RemoveInRange() { boxesInRangeCount = Mathf.Max(0, boxesInRangeCount - 1); }

    public void FreezeForTask() { IsBusy = true; SetActive(false); }
    public void Unfreeze(bool makeActive = false)
    {
        IsBusy = false;
        if (makeActive) SetActive(true);    // only if caller really wants control back
    }
}
