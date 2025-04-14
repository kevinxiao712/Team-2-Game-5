using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script does not currently actually do anything
public class MoveBandMember : MonoBehaviour
{
    public GameObject[] bandMembers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveMember()
    {
        int randMember = Random.Range(0, bandMembers.Length);
        bandMembers[randMember].transform.position = new Vector3(bandMembers[randMember].transform.position.x, bandMembers[randMember].transform.position.y + 3, 0);
        bandMembers[randMember].transform.position = new Vector3(bandMembers[randMember].transform.position.x, bandMembers[randMember].transform.position.y - 3, 0);
    }
}
