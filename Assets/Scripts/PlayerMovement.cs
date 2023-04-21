using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private static string TAG = "PlayerMovement";
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float walkSpeed = 7f;
    float dirX = 0f;
    private Animator anim;

    private enum MovementState {
        IDLE,
        RUNNING,
        JUMPING,
        FALLING
    }

    private SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(TAG + " # Start");

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // place the player back on the platform if it falls
        if (rb.transform.position.y < -10) {
            rb.transform.position = new Vector2(0, 0.2f);
            rb.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        // set the input horizontal axis raw value
        dirX = Input.GetAxisRaw("Horizontal");
        // move the player corresponding to the horizontal axis input, based on its walkspeed
        rb.velocity = new Vector2(dirX * walkSpeed, rb.velocity.y);

        // when pressing the Jump button
        if (Input.GetButtonDown("Jump") && isGrounded()) 
        {
            // move the player on the vertical axis by its jumpForce
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimation(dirX);
    }

    private void UpdateAnimation(float dirX) {
        MovementState state; 

        if (dirX > 0f) { // the player is moving in positive direction
            state = MovementState.RUNNING;
            spriteRenderer.flipX = false;
        } else if (dirX < 0f) { // negative direction
            state = MovementState.RUNNING;
            spriteRenderer.flipX = true;
        } else { // idle
            state = MovementState.IDLE;
        }

        // check for jumping
        if (rb.velocity.y > .1f) {// positive y velocity
            state = MovementState.JUMPING;
        } else if (rb.velocity.y < -.1f) {// negative y velocity
            state = MovementState.FALLING;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded() {
        // creates a box of the same shape with the box collider
        // checks if it overlaps with the jumpableGround
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
