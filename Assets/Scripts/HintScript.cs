using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintScript : MonoBehaviour
{
    public void OnMouseDown()
    {
        // Code to execute when the object is clicked
        Debug.Log("Hint Clicked!");
        GameController.Instance.playHint();
    }
}
