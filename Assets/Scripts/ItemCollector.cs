using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{

    private GameController gameControllerObj = GameController.Instance;
    [SerializeField] private GameController.Collectible collectible;
    [SerializeField] private GameController.Collectible optionalCollectible = GameController.Collectible.None;
    [SerializeField] private GameController.Collectible wrongCollectible = GameController.Collectible.None;

    private AudioSource audioSource;
    
    void Start() {
        audioSource = new GameObject("AudioObject").AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Audio/wrong");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        gameControllerObj = GameController.Instance;
        // Debug.Log("Collision "+collision.tag);
        // Send the collision object to the GameController if it is one of the desired collisions
        if (collision.tag == collectible.ToString())
        {
            gameControllerObj.collected(collectible, collision.gameObject);
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
        if (collision.tag == optionalCollectible.ToString())
        {
            gameControllerObj.collected(optionalCollectible, collision.gameObject);
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
        if (collision.tag == wrongCollectible.ToString()) {
            gameControllerObj.collected(wrongCollectible, collision.gameObject);
            audioSource.Play();
        }
    }
}
