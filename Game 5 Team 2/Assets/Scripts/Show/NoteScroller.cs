using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    public float speed;
    public bool hasStarted;
    public int currentScore;

    public Canvas instructions;

    // Start is called before the first frame update
    void Start()
    {

        //if there is a score manager and the score is greater than 0, set the speed to that score (not sure how score works yet)
        if (ScoreManager.Instance != null && ScoreManager.Instance.currentScore >= 0) //change this to just > 0 if you wanna test on 2 (intended speed), 4 is DIFFICULT
        {
            currentScore = ScoreManager.Instance.currentScore;

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
        
    }
}
