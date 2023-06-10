using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameAudioController : MonoBehaviour
{
    public static MinigameAudioController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private AudioSource audioSrc;
    private AudioSource wrongAudioSrc;
    void Start() {
        wrongAudioSrc = new GameObject("AudioObject").AddComponent<AudioSource>();
        wrongAudioSrc.clip = Resources.Load<AudioClip>("Audio/wrong");
        audioSrc = GetComponent<AudioSource>();
        audioSrc.Play();
    }

    public void startVoice(AudioClip audio) {
        StartCoroutine(WaitForFirstClipToEnd(audio));
    }

    public void playWrongSound() {
        wrongAudioSrc.Play();
    }

    private IEnumerator WaitForFirstClipToEnd(AudioClip audio)
    {
        yield return new WaitWhile(() => audioSrc.isPlaying);
        audioSrc.clip = audio;
        audioSrc.Play();
    }
}
