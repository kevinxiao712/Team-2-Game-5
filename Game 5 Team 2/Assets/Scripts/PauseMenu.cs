using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public enum GamePhase
    {
        Prep,
        PreShow,
        Show,
        Postshow
    }

    [SerializeField]
    private GameObject menu;
    private GamePhase currentPhase;

    // Kinda scuffed implementation because we don't have a universal manager script
    [SerializeField]
    private Canvas preShowCanvas;
    private GameManager preShowManager;
    [SerializeField]
    private Canvas showCanvas;
    // Insert showManager when that's a thing
    [SerializeField]
    private Canvas postShowCanvas; 
    private PostShowManager postShowManager;

    // Start is called before the first frame update
    void Start()
    {
        currentPhase = GamePhase.Prep;
        preShowManager = FindAnyObjectByType<GameManager>();
        postShowManager = FindAnyObjectByType<PostShowManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePauseMenu(bool enabled)
    {
        menu.SetActive(enabled);
        switch (currentPhase)
        {
            case GamePhase.Prep:
                return;
            case GamePhase.PreShow:
                preShowCanvas.enabled = false;
                return;
        }
    }

    public void IncrementPhase()
    {
        currentPhase = (GamePhase)((int)(currentPhase + 1) % 4);
    }
}
