using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumbersScript : MonoBehaviour
{
    [SerializeField] private bool isCorrect = false;
    [SerializeField] private int minigameNumber = 1;


    private void OnMouseDown() {
        if (!isCorrect) {
            MinigameAudioController.Instance.playWrongSound();
        }
        if(minigameNumber == 1) {
            GameController.Instance.minigame2Pressed(isCorrect);
        }
        else {
            GameController.Instance.minigame3Pressed(isCorrect);
        }
    }
}
