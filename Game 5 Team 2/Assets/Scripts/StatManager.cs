using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    // Need to wait for Discord outage to end to clarify what the stats are
    public struct InstrumentStat
    {
        public int InstrumentSkill; // How much this instrument scores during performances
        public int InstrumentEase; // How lenient the timing is for minigames with this instrument
    }

    // ~~~ MANAGER STATS ~~~
    [SerializeField]
    private int managerialSkills = 5; // Affects various managerial tasks(?)
    [SerializeField]
    private int showPresence = 5; // Affects how well a show does(?)
    [SerializeField]
    private int fanInteractions = 5; // Affects the success rate when interacting with fans(?)

    // ~~~ BAND STATS ~~~
    public InstrumentStat drumStats;
    public InstrumentStat guitarStats;
    public InstrumentStat bassStats;

    // Currently equipped items
    [SerializeField]
    private ItemScriptableObject managerItem1;
    [SerializeField]
    private ItemScriptableObject managerItem2;
    [SerializeField]
    private ItemScriptableObject managerItem3;

    // Getters for the current stats
    public int ManagerialSkills
    {
        get { return managerialSkills + managerItem1.managerialSkillsMod; }
    }

    public int ShowPresence
    {
        get { return showPresence + managerItem2.showPresenceMod; }
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
