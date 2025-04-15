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
    // Start is called before the first frame update
    void Start()
    {
        itemName = GetComponentInChildren<TextMeshProUGUI>().text;
        if (itemName == "None") return;
        Toggle toggle = GetComponent<Toggle>();
        statManager = FindAnyObjectByType<StatManager>();
        foreach (ItemScriptableObject item in statManager.ManagerItems)
        {
            if (item != null && item.itemName == itemName)
                toggle.interactable = false;
        }

        if (descriptionBox != null)
            descriptionBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        if (itemName == "None" || descriptionBox == null) return; 
        descriptionBox.GetComponentInChildren<TextMeshProUGUI>().text =
            statManager.FindItemOfName(itemName).description;
        descriptionBox.SetActive(true);
    }

    public void OnPointerExit()
    {
        if (itemName == "None" || descriptionBox == null) return;
        descriptionBox.SetActive(false);
    }
}
