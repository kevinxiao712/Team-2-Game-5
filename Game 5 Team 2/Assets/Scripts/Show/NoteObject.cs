using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    //variables
    public bool canPress;

    public KeyCode keyToPress;
    public KeyCode otherKeyToPress;

    public MoveBandMember moveBandMember;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = gameObject.transform.position;
        moveBandMember = GetComponent<MoveBandMember>();
    }

    // Update is called once per frame
    void Update()
    {
        //if arrow is pressed while inside a button, disable it
        if(Input.GetKeyDown(keyToPress) || Input.GetKeyDown(otherKeyToPress))
        {
            if(canPress)
            {
                 gameObject.SetActive(false);

                 NoteHit();
            }
        }
    }

    //if arrow enters a button, allow it to be pressed
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canPress = true;
        }
    }

    //if arrow exits a button, disallow it to be pressed
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canPress = false;

            NoteMiss();
        }
    }

    public void NoteHit()
    {
        Debug.Log("Note Hit");
        ScoreManager.Instance.AddNoteHit();
    }

    public void NoteMiss()
    {
        Debug.Log("Note Missed");
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.red;
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(Despawn());
        }
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }

    public void ResetNote()
    {
        transform.position = startPos;
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.white;
        gameObject.SetActive(true);
        canPress = false;
    }
}
