using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    private bool dragging = false;
    private Vector3 offset = new Vector3(0, 0, 0);

    // Update is called once per frame
    void Update()
    {
        if(dragging) {
            // move the object
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            GetComponent<MoveObjectsBackToStart>().SetIsBeingDragged(true);
        }
    }

    private void OnMouseDown() {
        // record the difference between the object's centre and the clicked point in the camera plane
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
        
    }

    private void OnMouseUp() {
        // stop dragging
        dragging = false;
        GetComponent<MoveObjectsBackToStart>().SetIsBeingDragged(false);
    }

    public void setDragging(bool isDragging) {
        dragging = isDragging;
        
    }
}
