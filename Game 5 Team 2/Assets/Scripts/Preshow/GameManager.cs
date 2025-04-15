using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("References to our two characters")]
    public CharacterController2D characterA; 
    public CharacterController2D characterB;  

    // This reference will track which character is currently active.
    private CharacterController2D currentActive;

    [Header("UI Canvases")]
    public Canvas PrepmainCanvas;       
    public GameObject PreshowCanvas;
    public GameObject ThirdPhaseObject;

    [Header("Preshow Timer UI")]
    public TextMeshPro preshowTimerText;


    [Header("Timer Settings")]
    public float preshowDuration = 300f; // 5 minutes = 300 seconds
    private float preshowTimeLeft;
    private bool isPreshowActive = false;


    private void Start()
    {
        currentActive = characterA;
        characterA.SetActive(true);
        characterB.SetActive(false);

        if (ThirdPhaseObject != null)
            ThirdPhaseObject.SetActive(false);
    }

    private void Update()
    {
        // Allow Tab switching
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchActiveCharacter();
        }

        if (isPreshowActive)
        {
            Debug.Log(preshowTimeLeft);

            preshowTimeLeft -= Time.deltaTime;
            if (preshowTimeLeft <= 0f)
            {
                EndPreshow();
            }
            else
            {
                // Update the text to show MM:SS
                if (preshowTimerText != null)
                {
                    preshowTimerText.text = FormatTime(preshowTimeLeft);
                    Debug.Log(preshowTimeLeft);
                }
            }
        }
    }

    private void SwitchActiveCharacter()
    {
        // Deactivate the current
        currentActive.SetActive(false);

        // Switch references
        if (currentActive == characterA)
        {
            currentActive = characterB;
        }
        else
        {
            currentActive = characterA;
        }

        // Activate the newly selected character
        currentActive.SetActive(true);
    }
    public void OnSwitchCanvasButtonClicked()
    {
        // Disable the Prep canvas
        PrepmainCanvas.gameObject.SetActive(false);

        // Enable the Preshow Canvas
        PreshowCanvas.gameObject.SetActive(true);

        // Initialize the timer
        preshowTimeLeft = preshowDuration;  // e.g., 300f for 5 minutes
        isPreshowActive = true;

        // Make sure the text is visible and reset
        if (preshowTimerText != null)
        {
            preshowTimerText.gameObject.SetActive(true);
            preshowTimerText.text = FormatTime(preshowTimeLeft);
        }
    }


    private void EndPreshow()
    {
        isPreshowActive = false;

        // Hide the Preshow Canvas
        PreshowCanvas.gameObject.SetActive(false);

        // Enable the next (third) phase
        if (ThirdPhaseObject != null)
        {
            ThirdPhaseObject.SetActive(true);
        }

    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
