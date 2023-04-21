using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketCollecting : MonoBehaviour
{
    private int strawberries = 0;
    [SerializeField] private Text strawberriesText;

    public GameController gameControllerObj;
    private void OnTriggerEnter2D(Collider2D collision) {
         // lazy instantiate the game controller
        if (gameControllerObj == null) {
            gameControllerObj = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
        Debug.Log("Collision "+collision.tag);
        if (collision.gameObject.CompareTag("Strawberry")) {
                // destroy the game object that collided with the bucket
                Destroy(collision.gameObject);
                strawberries++;
                gameControllerObj.collected(GameController.Collectible.STRAWBERRY);
                strawberriesText.text = "Capsuni: "+strawberries;
        }
    }
}
