using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int strawberriesCollected = 0;
    private int strawberryWinCnd = 2;
    private int cherriesCollected = 0;
    private int cherryWinCnd = 3;
    private int ripeAppleCollected = 0;
    private int ripeAppleWinCnd = 3;

    [SerializeField] Text winText;
    public enum Collectible {
        Cherry,
        Strawberry,
        Apple,
        RipeApple,
        None
    }

    // logic for working with the collectibles based on what object was taken to the basket
    public void collected(Collectible collectible, GameObject collidedWith) {
        Debug.Log("Collected "+collectible.ToString());
        switch (collectible) {
            case Collectible.Cherry:
            cherriesCollected++;
            // destroy the game object that collided with the bucket
            Destroy(collidedWith);
            break;
            case Collectible.Strawberry:
            strawberriesCollected++;
            Destroy(collidedWith);
            break;
            case Collectible.Apple:
            StartCoroutine(displayTextForSeconds("Acel mar nu este copt!", 3));
            break;
            case Collectible.RipeApple:
            ripeAppleCollected++;
            Destroy(collidedWith);
            break;
            default:
            break;
        }
        checkWinCnd();
    }

    private void checkWinCnd() {
        if (cherriesCollected >= cherryWinCnd &&
         strawberriesCollected >= strawberryWinCnd &&
         ripeAppleCollected >= ripeAppleWinCnd
         ) {
            winText.text = "GG, esti un stroomph!";
            winText.enabled = true;
        }
    }

    private IEnumerator displayTextForSeconds(string text, float seconds) {
        winText.text = text;
        winText.enabled = true; // Enable the text so it shows
        yield return new WaitForSeconds(seconds);
        winText.enabled = false; // Disable the text so it is hidden
        winText.text = "";
    }
}
