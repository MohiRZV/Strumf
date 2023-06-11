using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatShape : DragScript
{
    GameController gameController = GameController.Instance;
    [SerializeField] GameObject boat;
    [SerializeField] Vector3 squareOffset = new Vector3(1.7f, 1.3f, 0.0f);
    [SerializeField] Vector3 initialPosition = new Vector3(0.5f, -1.6f, -0.5f);
    private float tolerance = 0.3f;

    void Start() {
        if (gameController == null) {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    private void OnMouseUp() {
        setDragging(false);
        Vector3 position = this.transform.position;
        Vector3 targetPosition = boat.transform.position - squareOffset;
        Vector3 offset = targetPosition - position;
        //Debug.Log(position + " " + boat.transform.position + " " + targetPosition + " " + offset);
        if (Mathf.Abs(offset.x) < tolerance && Mathf.Abs(offset.y) < tolerance) {
            setDragging(false);
            transform.position = targetPosition;
            BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
            collider.enabled = false;
            gameController.collected(GameController.Collectible.BoatPart, null);
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
        else {
            gameController.collected(GameController.Collectible.BoatPartWrong, null);
            transform.position = initialPosition;
        }
    }
}
