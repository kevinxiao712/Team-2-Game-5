using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public GameObject pressFIndicator;
    private bool isActive;
    private int boxesInRangeCount = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (pressFIndicator != null)
            pressFIndicator.SetActive(false);
    }

    public void SetActive(bool active)
    {
        isActive = active;
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
    public void AddInRange()
    {
        boxesInRangeCount++;
        if (boxesInRangeCount > 0 && pressFIndicator != null)
        {
            pressFIndicator.SetActive(true);
        }
    }
    public void RemoveInRange()
    {
        boxesInRangeCount = Mathf.Max(0, boxesInRangeCount - 1);

        if (boxesInRangeCount == 0 && pressFIndicator != null)
        {
            pressFIndicator.SetActive(false);
        }
    }
}
