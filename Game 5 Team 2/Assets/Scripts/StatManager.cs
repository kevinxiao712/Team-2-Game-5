using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private ItemScriptableObject managerItem1;
    [SerializeField]
    private ItemScriptableObject managerItem2;
    [SerializeField]
    private ItemScriptableObject managerItem3;

    // Manager item inventory
    // This stuff will cause problems when StatManager becomes a singleton, will move some of this
    // to a SetupPhaseScene script when needed but for now I just want to make sure it works
    [SerializeField]
    private ItemScriptableObject[] managerItemInventory;
    [SerializeField]
    private TMP_Dropdown[] managerItemDropdowns;

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

    // Start is called before the first frame update
    void Start()
    {
        foreach (TMP_Dropdown dropdown in managerItemDropdowns)
        {
            dropdown.options.Clear();
            foreach (ItemScriptableObject item in managerItemInventory)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(item.itemName));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDropdownOptions(Int32 newOption)
    {

    }
}
