using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public enum CurrentPhase
    {
        Prep,
        Preshow,
        Show,
        Postshow,
        End
    }

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private GameObject prepCanvas;
    [SerializeField]
    private GameObject preshowCanvas;
    [SerializeField]
    private GameObject showCanvas;
    [SerializeField]
    private GameObject postshowCanvas;
    [SerializeField]
    private GameObject endgameCanvas;

    private CurrentPhase currentPhase;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        currentPhase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu(!menu.activeSelf);
        }
    }

    public void TogglePauseMenu(bool enabled)
    {
        menu.SetActive(enabled);
        switch (currentPhase) // Required since there's no universal game manager
        {
            case CurrentPhase.Prep:
                prepCanvas.SetActive(!enabled);
                break;
            case CurrentPhase.Preshow:
                preshowCanvas.SetActive(!enabled); 
                break;
            case CurrentPhase.Show:
                showCanvas.SetActive(!enabled);
                break;
            case CurrentPhase.Postshow:
                postshowCanvas.SetActive(!enabled);
                break;
            case CurrentPhase.End:
                endgameCanvas.SetActive(!enabled);
                break;
        }
        if (enabled)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void IncrementPhase()
    {
        currentPhase = (CurrentPhase)((int)(currentPhase + 1) % 5);
        Debug.Log(currentPhase.ToString());
    }
}
