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
    public GameObject instructionsCanvas;

    [Header("Preshow Timer UI")]
    public TextMeshPro preshowTimerText;
    private bool gamePaused = false;


    [Header("Timer Settings")]
    public float preshowDuration = 300f; // 5 minutes = 300 seconds
    private float preshowTimeLeft;
    private bool isPreshowActive = false;
    private bool isInstructionOpen = false;

    private void Start()
    {
        currentActive = characterA;
        characterA.SetActive(true);
        characterB.SetActive(false);

        if (ThirdPhaseObject != null)
            ThirdPhaseObject.SetActive(false);

        if (instructionsCanvas != null)
            instructionsCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchActiveCharacter();
        }

        // If preshow is active AND the instructions are not open, count down
        if (isPreshowActive && !isInstructionOpen)
        {
            preshowTimeLeft -= Time.deltaTime;
            if (preshowTimeLeft <= 0f)
            {
                EndPreshow();
            }
            else
            {
                if (preshowTimerText != null)
                {
                    preshowTimerText.text = FormatTime(preshowTimeLeft);
                }
            }
        }

        // If the instructions are open, wait for key press to close it
        if (isInstructionOpen && Input.GetKeyDown(KeyCode.Space))
        {
            CloseInstructions();
        }
    }

    private void CloseInstructions()
    {
        if (instructionsCanvas != null)
            instructionsCanvas.SetActive(false);

        isInstructionOpen = false;

        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1f;           // resume everything
            currentActive.SetActive(true);
        }
    }
    public void SwitchActiveCharacter()
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
        ResetPreshow();

        FindAnyObjectByType<PauseMenu>().IncrementPhase();

        // Disable the Prep canvas
        PrepmainCanvas.gameObject.SetActive(false);

        // Enable the Preshow
        PreshowCanvas.gameObject.SetActive(true);

        // Show the instructions right away
        if (instructionsCanvas != null)
        {
            instructionsCanvas.SetActive(true);
            isInstructionOpen = true;

            gamePaused = true;
            Time.timeScale = 0f;             
            characterA.SetActive(false); 
            characterB.SetActive(false);
        }


        // Initialize the timer but don't start counting down yet
        preshowTimeLeft = preshowDuration;
        isPreshowActive = true;

        // Update the UI text to show full time
        if (preshowTimerText != null)
        {
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
            FindAnyObjectByType<PauseMenu>().IncrementPhase();
            ThirdPhaseObject.SetActive(true);
        }

    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetPreshow()
    {

        if (PrepmainCanvas != null) PrepmainCanvas.gameObject.SetActive(true);
        if (PreshowCanvas != null) PreshowCanvas.SetActive(false);
        if (ThirdPhaseObject != null) ThirdPhaseObject.SetActive(false);
        if (instructionsCanvas != null) instructionsCanvas.SetActive(false);

        isPreshowActive = false;
        isInstructionOpen = false;
        preshowTimeLeft = preshowDuration;
        if (preshowTimerText != null)
            preshowTimerText.text = FormatTime(preshowTimeLeft);

        if (currentActive != null)
            currentActive.SetActive(false);

        currentActive = characterA;
        characterA.SetActive(true);
        characterB.SetActive(false);
    }
}
