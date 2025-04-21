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

                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.8f, LayerMask.GetMask("Character"));

                foreach (Collider2D h in hits)
                {
                    // ignore our own collider
                    if (h.gameObject == gameObject) continue;


                    if (h.GetComponent<PickUpable>() != null)
                    {
                        Debug.Log("Picked up " + h.name);
                        carried = h.gameObject;

                        // wake up rigid‑body
/*                        var rb = carried.GetComponent<Rigidbody2D>();
                        if (rb != null)
                        {
                            rb.velocity = Vector2.zero;
                            rb.angularVelocity = 0f;
                            rb.bodyType = RigidbodyType2D.Kinematic;
                            rb.simulated = true;
                        }*/

                        carried.GetComponent<CharacterController2D>().SetActive(false);
                        carried.transform.SetParent(followOffset, worldPositionStays: false);
                        carried.transform.localPosition = Vector3.zero;
                        break;                   
                    }
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
