using UnityEngine;

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
    private void Start()
    {
        currentActive = characterA;
        characterA.SetActive(true);
        characterB.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchActiveCharacter();
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
        // Disable the first canvas
        PrepmainCanvas.gameObject.SetActive(false);

        // Enable the second canvas
        PreshowCanvas.gameObject.SetActive(true);
    }
}
