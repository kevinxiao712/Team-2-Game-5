using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

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
    private List<ItemScriptableObject> managerItemChoices = new();
    [SerializeField]
    private List<InstrumentScriptableObject> instrumentChoices = new();
    [SerializeField]
    private TMP_Dropdown[] managerItemDropdowns; 
    [SerializeField]
    private TMP_Dropdown[] instrumentDropdowns;

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
        RefreshDropdowns();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Very scuffed hard-coded values, will tweak to be more flexible later
    public void SetGuitar()
    {
        bandInstruments[0] = FindInstrumentOfName(instrumentDropdowns[0].options[instrumentDropdowns[0].value].text);
        Debug.Log(bandInstruments[0].itemName);
    }
    public void SetBass()
    {
        bandInstruments[1] = FindInstrumentOfName(instrumentDropdowns[1].options[instrumentDropdowns[1].value].text);
        Debug.Log(bandInstruments[1].itemName);
    }
    public void SetDrums()
    {
        bandInstruments[2] = FindInstrumentOfName(instrumentDropdowns[2].options[instrumentDropdowns[2].value].text);
        Debug.Log(bandInstruments[2].itemName);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void SetInventoryItem(int slot)
    {
        managerItems[slot] = managerItemChoices[managerItemDropdowns[slot].value];
    }

    public ItemScriptableObject FindItemOfName(string name)
    {
        foreach (ItemScriptableObject item in managerItemChoices)
        {
            if (item.itemName == name)
                return item;
        }
        return null;
    }
    public InstrumentScriptableObject FindInstrumentOfName(string name)
    {
        foreach (InstrumentScriptableObject inst in instrumentChoices)
        {
            if (inst.itemName == name)
                return inst;
        }
        return null;
    }

    public void AddItemToInventory(ItemScriptableObject item)
    {
        if (item is InstrumentScriptableObject && 
            !instrumentChoices.Contains((InstrumentScriptableObject)item))
            instrumentChoices.Add((InstrumentScriptableObject)item);

        else if (!(item is InstrumentScriptableObject) && !managerItemChoices.Contains(item))
            managerItemChoices.Add(item);

        Debug.Log(item.unlockFlavorText);
        RefreshDropdowns();
    }

    private void RefreshDropdowns()
    {
        // Update the three manager item dropdowns
        foreach (TMP_Dropdown dropdown in managerItemDropdowns)
        {
            dropdown.ClearOptions();
            foreach (ItemScriptableObject item in managerItemChoices)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(item.itemName));
            }
            dropdown.value = 0;
            dropdown.RefreshShownValue();
        }

        // Update each of the instrument dropdowns
        UpdateInstrumentDropdown(InstrumentScriptableObject.InstrumentType.Guitar);
        UpdateInstrumentDropdown(InstrumentScriptableObject.InstrumentType.Bass);
        UpdateInstrumentDropdown(InstrumentScriptableObject.InstrumentType.Drums);
    }

    private void UpdateInstrumentDropdown(InstrumentScriptableObject.InstrumentType type)
    {
        TMP_Dropdown dropdown = instrumentDropdowns[(int)type];
        dropdown.ClearOptions();
        foreach (InstrumentScriptableObject inst in instrumentChoices)
        {
            if (inst.instrumentType == type)
                dropdown.options.Add(new TMP_Dropdown.OptionData(inst.itemName));
        }
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    public static StatManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
