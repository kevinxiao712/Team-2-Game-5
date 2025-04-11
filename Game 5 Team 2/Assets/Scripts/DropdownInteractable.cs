using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownInteractable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string itemName = GetComponentInChildren<TextMeshProUGUI>().text;
        if (itemName == "None") return;
        Toggle toggle = GetComponent<Toggle>();
        StatManager statManager = FindAnyObjectByType<StatManager>();
        foreach (ItemScriptableObject item in statManager.CurrentManagerItems)
        {
            if (item != null && item.itemName == itemName)
                toggle.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
