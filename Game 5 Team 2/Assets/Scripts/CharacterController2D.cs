using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private bool isActive;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
}
