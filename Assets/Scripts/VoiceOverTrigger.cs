using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    Collider2D soundTrigger;
    [SerializeField] private AudioClip instruction;
    private AudioSource audioSrc;
    [SerializeField] private bool startMinigame = false;

    void Awake()
    {
        soundTrigger = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            audioSrc = GetComponent<AudioSource>();
            audioSrc.Play();
            if (instruction != null) {
                StartCoroutine(WaitForFirstClipToEnd());
            }

            if (startMinigame) {
                Debug.Log("voice over mini");
                StartCoroutine(StartMinigameAfterInstruction());
            }
        }
    }

    private IEnumerator WaitForFirstClipToEnd()
    {
        yield return new WaitWhile(() => audioSrc.isPlaying);
        audioSrc.clip = instruction;
        audioSrc.Play();
    }

    private IEnumerator StartMinigameAfterInstruction()
    {
        yield return new WaitWhile(() => audioSrc.isPlaying);
        GameController.Instance.startMinigame();
    }
}
