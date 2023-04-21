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

    [SerializeField] Text winText;
    public enum Collectible {
        CHERRY,
        STRAWBERRY
    }

    // mark the item as collected
    public void collected(Collectible collectible) {
        switch (collectible) {
            case Collectible.CHERRY:
            cherriesCollected++;
            break;
            case Collectible.STRAWBERRY:
            strawberriesCollected++;
            break;
            default:
            break;
        }
        checkWinCnd();
    }

    private void checkWinCnd() {
        if (cherriesCollected >= cherryWinCnd && strawberriesCollected >= strawberryWinCnd) {
            winText.enabled = true;
        }
    }
}
