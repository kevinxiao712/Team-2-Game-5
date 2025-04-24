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
        if (enabled)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void IncrementPhase()
    {
        currentPhase = (CurrentPhase)((int)(currentPhase + 1) % 5);
    }
}
