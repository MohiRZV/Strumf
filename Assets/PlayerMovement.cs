using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float jumpForce = 14f;
    private float walkForce = 5f;
    private static string TAG = "PlayerMovement";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(TAG + " Start");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * walkForce, rb.velocity.y);

        if (Input.GetButtonDown("Jump")) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
