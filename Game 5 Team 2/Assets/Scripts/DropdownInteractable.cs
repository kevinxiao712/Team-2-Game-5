using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownInteractable : MonoBehaviour
{
    [SerializeField]
    private GameObject descriptionBox;
    private string itemName;
    private StatManager statManager;
    [SerializeField]
    private bool isInstrumentDropdown;
    // Start is called before the first frame update
    void Start()
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>().text;
        statManager = FindAnyObjectByType<StatManager>();
        if (descriptionBox != null)
            descriptionBox.SetActive(false);

        if (itemName == "None" || isInstrumentDropdown) return;
        Toggle toggle = GetComponent<Toggle>();
        foreach (ItemScriptableObject item in statManager.ManagerItems)
        {
            if (item != null && item.itemName == itemName)
                toggle.interactable = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        if (itemName == "None" || descriptionBox == null) return;

        string descriptionText;
        if (isInstrumentDropdown)
            descriptionText = statManager.FindInstrumentOfName(itemName).description;
        else
            descriptionText = statManager.FindItemOfName(itemName).description;

        descriptionBox.GetComponentInChildren<TextMeshProUGUI>().text = descriptionText;
        descriptionBox.SetActive(true);
    }

    public void OnPointerExit()
    {
        if (itemName == "None" || descriptionBox == null) return;
        descriptionBox.SetActive(false);
    }
}
