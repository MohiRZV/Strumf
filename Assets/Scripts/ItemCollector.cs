using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private string TAG = "ItemCollector";
    private int cherries = 0;
    private int heldCherries = 0;

    [SerializeField] private Text cherriesText;

    public GameController gameControllerObj;
    private void OnTriggerEnter2D(Collider2D collision) {
        
        // lazy instantiate the game controller
        if (gameControllerObj == null) {
            gameControllerObj = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        Debug.Log("Collision "+collision.tag);
        if (collision.gameObject.CompareTag("Cherry")) {
            // can't carry more than one cherry at a time
            if (heldCherries<1) {
                // destroy the game object the player collided with
                Destroy(collision.gameObject);
                heldCherries++;
                gameControllerObj.collected(GameController.Collectible.CHERRY);
            } else {
                Debug.Log(TAG + "# can't pick up more than one cherry at a time");
            }
        }


        if (collision.gameObject.CompareTag("Box")) {
            // deposit the cherry
            if (heldCherries > 0) {
                cherries++;
                // display the number of collected cherries
                cherriesText.text = "Cirese: " + cherries;
                heldCherries--;
            } else {
                Debug.Log(TAG + "# nothing to deposit");
            }
        }
    }
}
