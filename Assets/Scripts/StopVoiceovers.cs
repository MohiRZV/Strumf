using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVoiceovers : MonoBehaviour
{
    public GameObject gameObject;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}

