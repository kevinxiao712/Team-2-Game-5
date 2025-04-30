using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteScroller : MonoBehaviour
{

    public GameObject showPhaseObject;    
    public GameObject postShowPhaseObject;
    public float speed;
    public bool hasStarted;
    public int currentScore;

    public Canvas instructions;
    [SerializeField]
    private List<GameObject> activeNotes;
    [SerializeField]
    private List<GameObject> inactiveNotes;

    // Start is called before the first frame update
    void Start()
    {

        //if there is a score manager and the score is greater than 0, set the speed to that score (not sure how score works yet)
        if (ScoreManager.Instance != null && ScoreManager.Instance.preshowScore >= 0) //change this to just > 0 if you wanna test on 2 (intended speed), 4 is DIFFICULT
        {
            currentScore = ScoreManager.Instance.preshowScore;

            //make the game easier the higher the score the player has (right now it is VERY hard if it's above 2)
            if (currentScore >= 0 && currentScore <= 20)
            {
                Debug.Log("Score Manager found, setting speed to 4bps");
                speed = 4;
            }

            if (currentScore > 20 && currentScore <= 50)
            {
                Debug.Log("Score Manager found, setting speed to 3bps");
                speed = 3;
            }

            if (currentScore > 50 && currentScore <= 100)
            {
                Debug.Log("Score Manager found, setting speed to 2bps");
                speed = 2;
            }

            if (currentScore > 100)
            {
                Debug.Log("Score Manager found, setting speed to 1bps");
                speed = 1;
            }
        }
        //if currentScore isn't available, just make it 2
        else
        {
            //normal tempo = 120bpm, 120bpm/60s = 2bps
            speed = 2;
            Debug.Log("Score Manager not found, setting speed to 2bps");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //when space is pressed, start the scrolling
        if(!hasStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                instructions.enabled = false;
                hasStarted = true;
            }
        }

        //translate all arrows down the screen
        else
        {
            transform.position -= new Vector3(0f, speed * Time.deltaTime, 0f);
        }

        //add note to new list and remove it from old one if it is inactive
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            var go = activeNotes[i];
            if (!go.activeSelf)
            {
                inactiveNotes.Add(go);
                activeNotes.RemoveAt(i);
            }
        }

        //whatever transition goes here (put it in the coroutine actually)
        if (activeNotes.Count <= 0)
        {
            StartCoroutine(Change());
        }
        
    }

    //reset all of the notes
    public void ResetScroller()
    {
        hasStarted = false;
        instructions.enabled = true;

        foreach (GameObject go in inactiveNotes)
        {
            activeNotes.Add(go);
            go.GetComponent<NoteObject>().ResetNote();
            Debug.Log(go.name + "has been reset");
        }

        inactiveNotes.Clear();
        ScoreManager.Instance.showScore = 0;
        if (ScoreManager.Instance.showScoreText != null)
        {
            ScoreManager.Instance.showScoreText.text = "Show Score: " + ScoreManager.Instance.showScore;
        }
        
        PostShowManager.Instance.ResetPostShow();
    }

    private IEnumerator Change()
    {
        yield return new WaitForSeconds(3f);

        // 1) Kick off the PostShow while we're still active
        PostShowManager.Instance.BeginPostShow();
     //   FindAnyObjectByType<PauseMenu>().IncrementPhase();

        // 2) Switch UI
        if (postShowPhaseObject != null)
            postShowPhaseObject.SetActive(true);

        if (showPhaseObject != null)
            showPhaseObject.SetActive(false);
    }
}
