using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class MoveObjectsBackToStart : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isBeingDragged;

    void Start()
    {
        startPosition = transform.position;
        isBeingDragged = false;
    }

    void Update()
    {
        if (!isBeingDragged && (transform.position - startPosition).magnitude > 2f)
        {
            transform.position = startPosition;
        }
    }

    public void SetIsBeingDragged(bool value)
    {
        isBeingDragged = value;
    }
}
