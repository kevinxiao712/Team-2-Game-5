using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    //variables
    [SerializeField]
    private Image img;

    //these are for if we have separate images for presses, rn just color change
    //public Sprite defaultImage;
    //public Sprite pressedImage;
    [SerializeField]
    private Color normal;
    [SerializeField]
    private Color pressed;

    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        normal = img.color;
        pressed = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            img.color = pressed;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            img.color = normal;
        }
    }
}
