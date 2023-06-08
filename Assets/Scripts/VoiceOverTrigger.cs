using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    Collider2D soundTrigger;
    void Awake()
    {
        soundTrigger = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }

}
