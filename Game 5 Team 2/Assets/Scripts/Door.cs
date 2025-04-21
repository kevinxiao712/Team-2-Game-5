using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private int numPlayers = 0;
    private SpriteRenderer sr;

    [SerializeField] private Sprite[] doorStates;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            numPlayers++;
            sr.sprite = doorStates[1];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            numPlayers--;
            sr.sprite = doorStates[0];
        }
    }
}
