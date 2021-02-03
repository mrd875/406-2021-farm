using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // basic movement variables
    public float moveSpeed = 5.0f;
    public Vector2 movement;
    private bool isMoving = false;

    // Player RB to add movement to
    public Rigidbody2D rb;


    void Update()
    {
        // Get input for movement from WASD or Arrow keys
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (movement != new Vector2(0, 0) && isMoving == false)
        {
            isMoving = true;
        }
        else if (movement == new Vector2(0, 0) && isMoving == true)
        {
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        //Move in response to input from WASD or Arrow Keys
        //rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime); // old way: does not update velocity so changed
        rb.velocity = movement.normalized * moveSpeed * Time.fixedDeltaTime;

    }
}
