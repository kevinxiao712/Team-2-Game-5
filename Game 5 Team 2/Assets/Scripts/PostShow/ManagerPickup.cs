using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class RoadieController : MonoBehaviour
{
    public KeyCode pickupKey = KeyCode.F;
    public Transform followOffset;     
    private CharacterController2D mover;
    private GameObject carried;

    void Awake()
    {
        mover = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        if (carried == null)
        {
            if (Input.GetKeyDown(pickupKey))
            {

                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,
                                                              0.8f,
                                                              LayerMask.GetMask("Character"));

                foreach (Collider2D h in hits)
                {
                    if (h.gameObject == gameObject) continue;               // skip self
                    if (h.GetComponent<PickUpable>() == null) continue;     // need tag

                    carried = h.gameObject;
                    Debug.Log("Picked up " + carried.name);

                    // 1)  Disable physics & collisions on the carried object
                    var rb = carried.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                        rb.simulated = false;                 // physics off
                    }
                    var col = carried.GetComponent<Collider2D>();
                    if (col != null) col.enabled = false;        // collider off

                    // 2)  Disable its own controller
                    carried.GetComponent<CharacterController2D>().SetActive(false);

                    // 3)  Parent so it visually follows
                    carried.transform.SetParent(followOffset, worldPositionStays: false);
                    carried.transform.localPosition = Vector3.zero;
                    break;
                }

            }
        }
        else
        {
            carried.transform.localPosition = Vector3.zero; // keep it snapped
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Exit zone has tag "Exit"
        if (other.CompareTag("Exit") && carried != null)
        {
            Destroy(carried);    
            PostShowManager.Instance.ReportWrongRemoved(); 
            carried = null;
        }
    }
}
