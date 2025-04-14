using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    // ~~~ MANAGER STATS ~~~
    [SerializeField]
    private int stagePrep = 5; // Decreases time for prep tasks
    [SerializeField]
    private int contractNegotiation = 5; // Increases payout (whatever that means)
    [SerializeField]
    private int crowdWrangling = 5; // Decreases time for security tasks

    // ~~~ BAND STATS ~~~
    [SerializeField]
    private int showPresence = 5; // Improves leniency during minigames, increases final score
    [SerializeField]
    private int fanInteractions = 5; // Improves efficiency for fan interactions
    [SerializeField]
    private int vocalControl = 5; // Decreases time spent during practice, improves final score

    // Currently equipped items
    [SerializeField]
    private ItemScriptableObject[] managerItems = new ItemScriptableObject[3];
    // Index 0 is for guitar, index 1 is for bass, and index 2 is for drums
    [SerializeField]
    private InstrumentScriptableObject[] bandInstruments = new InstrumentScriptableObject[3];

    // Manager item inventory
    // This stuff will cause problems when StatManager becomes a singleton, will move some of this
    // to a SetupPhaseScene script when needed but for now I just want to make sure it works
    [SerializeField]
    private ItemScriptableObject[] managerItemChoices;
    [SerializeField]
    private InstrumentScriptableObject[] instrumentChoices;
    [SerializeField]
    private TMP_Dropdown[] managerItemDropdowns;

    // TEMPORARY: The scene to load when the "next" button is pressed
    [SerializeField]
    private string nextSceneName;

    // Getters for the current stats, will incorporate the currently equipped items at some point
    public int StagePrep
    {
        get { return stagePrep; }
    }

    public int ContractNegotiation
    {
        get { return contractNegotiation; }
    }

    public int CrowdWrangling
    { 
        get { return crowdWrangling; } 
    }

    public int ShowPresence
    { 
        get { return showPresence; } 
    }

    public int FanInterations
    { 
        get { return fanInteractions; } 
    }

    public int VocalControl
    { 
        get { return vocalControl; } 
    }

    public ItemScriptableObject[] ManagerItems
    {
        get
        {
            return managerItems;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (TMP_Dropdown dropdown in managerItemDropdowns)
        {
            dropdown.ClearOptions();
            foreach (ItemScriptableObject item in managerItemChoices)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(item.itemName));
            }
            dropdown.value = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // I don't like doing individual methods for each dropdown menu,
    // but Unity UI is annoying to work with in that regard so here I am
    public void SetInventoryItemOne(Int32 item)
    {
        SetInventoryItem(0, managerItemChoices[item]);
    }
    public void SetInventoryItemTwo(Int32 item)
    {
        SetInventoryItem(1, managerItemChoices[item]);
    }
    public void SetInventoryItemThree(Int32 item)
    {
        SetInventoryItem(2, managerItemChoices[item]);
    }
    // Very scuffed hard-coded values, will tweak to be more flexible later
    public void SetGuitar(Int32 type)
    {
        bandInstruments[0] = instrumentChoices[type];
    }
    public void SetBass(Int32 type)
    {
        bandInstruments[1] = instrumentChoices[type + 3];
    }
    public void SetDrums(Int32 type)
    {
        bandInstruments[0] = instrumentChoices[type + 6];
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void SetInventoryItem(int slot, ItemScriptableObject item)
    {
        managerItems[slot] = item;
    }
}
