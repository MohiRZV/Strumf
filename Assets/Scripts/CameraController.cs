using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // this is the player object !!!SHOULD BE SET IN THE EDITOR!!!
    [SerializeField] private Transform player;
    void Update()
    {
        // have the camera follow the player, but not it's rotation
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);

    }
}
