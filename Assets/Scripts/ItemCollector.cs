using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{

    public GameController gameControllerObj;
    [SerializeField] private GameController.Collectible collectible;
    [SerializeField] private GameController.Collectible wrongCollectible = GameController.Collectible.None;
    private void OnTriggerEnter2D(Collider2D collision) {
        // lazy instantiate the game controller
        if (gameControllerObj == null) {
            gameControllerObj = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        Debug.Log("Collision "+collision.tag);
        // Send the collision object to the GameController if it is one of the desired collisions
        if (collision.tag == collectible.ToString()) {
            gameControllerObj.collected(collectible, collision.gameObject);
        }
        if (collision.tag == wrongCollectible.ToString()) {
            gameControllerObj.collected(wrongCollectible, collision.gameObject);
        }
    }
}
