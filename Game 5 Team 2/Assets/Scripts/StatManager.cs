using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
