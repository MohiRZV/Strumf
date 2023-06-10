using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private static string TAG = "PlayerMovement";
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private bool enableKeyboardMovement = false;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private static Vector3 startPoint = new Vector3(-14f, 0f, -9f);
    [SerializeField] private static Vector3 checkPoint1 = new Vector3(5f, 0f, 0f);
    [SerializeField] private static Vector3 checkPoint2 = new Vector3(22f, 0f, 0f);
    [SerializeField] private static Vector3 checkPoint3 = new Vector3(37f, 0f, 0f);
    [SerializeField] private static Vector3 checkPoint4 = new Vector3(43f, 0f, 0f);
    [SerializeField] private static Vector3 checkPoint5 = new Vector3(50.5f, 0f, 0f);

    private static Vector3[] checkpoints = new Vector3[]{startPoint, checkPoint1, checkPoint2, checkPoint3, checkPoint4, checkPoint5};
    private int checkpointCount = checkpoints.Length; 
    private int nextCheckpoint = 1;
    [SerializeField] private bool advanceToNextCheckpointFlag = false;


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
        Debug.Log("Start from playermovement");
        checkpoints = new Vector3[]{startPoint, checkPoint1, checkPoint2, checkPoint3, checkPoint4, checkPoint5};
        checkpointCount = checkpoints.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (advanceToNextCheckpointFlag && (nextCheckpoint == 3 || nextCheckpoint == 4)) {// we need to move the boat
            //Debug.Log("move boat");
            Rigidbody2D boatRb = GameObject.FindGameObjectWithTag("Boat").GetComponent<Rigidbody2D>();
            float dir = getMovementDir();
            //Debug.Log(dir);
            float shake = Random.Range(-0.2f, 0.2f);
            boatRb.velocity = new Vector2(getMovementDir() * walkSpeed, boatRb.velocity.y);
            return;
        }
        // place the player back on the platform if it falls
        if (rb.transform.position.y < -10) {
            rb.transform.position = new Vector2(0, 0.2f);
            rb.transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        // set the input horizontal axis raw value
        if(enableKeyboardMovement) {
            dirX = Input.GetAxisRaw("Horizontal");
        } else {
            dirX = getMovementDir();
        }

        // move the player corresponding to the horizontal axis input, based on its walkspeed
        rb.velocity = new Vector2(dirX * walkSpeed, rb.velocity.y);

        // when pressing the Jump button
        if (enableKeyboardMovement && Input.GetButtonDown("Jump") && isGrounded()) 
        {
            // move the player on the vertical axis by its jumpForce
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimation(dirX);
    }

    private float getMovementDir() {
        if (advanceToNextCheckpointFlag) {
            if (transform.position.x - checkpoints[nextCheckpoint].x > 0) {//checkpoint reached
                transform.position = new Vector3(checkpoints[nextCheckpoint].x, transform.position.y, transform.position.z);
                nextCheckpoint++;
                advanceToNextCheckpointFlag = false;
            } else {
                return 1f;
            }
        }
        return 0f;
    }

    public void advanceToNextCheckpoint() {
        advanceToNextCheckpointFlag = true;
    }

    public void placeToCheckpoint(int checkpoint) {
        Debug.Log("Checkpoint no "+checkpoint);
        nextCheckpoint = checkpoint+1;
        transform.position = new Vector3(checkpoints[checkpoint].x, transform.position.y, transform.position.z);
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
