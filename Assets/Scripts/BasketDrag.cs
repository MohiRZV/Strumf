using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketDrag : MonoBehaviour
{
    private Vector3 initialPosition;

    private void OnMouseDown()
    {
        initialPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;
        float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 newPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, distanceToScreen)).x;

        transform.position = newPosition;
    }
}
