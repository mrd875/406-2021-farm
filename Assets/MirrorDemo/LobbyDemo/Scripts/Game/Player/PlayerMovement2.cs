using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement2 : NetworkBehaviour
{
    // basic movement variables
    public float moveSpeed = 200.0f;
    private float baseMoveSpeed;
    private bool isMoving = false;
    private int appliedReductionEffects; // amount of slow effects applied (for duration purpose)
    public Vector2 movement;


    // Player Rigidbody component to add movement to
    [HideInInspector]
    public Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (hasAuthority)
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

        // enable transformation of player's horizontal face direction
        float horizontalDirection = Input.GetAxisRaw("Horizontal");
        if (horizontalDirection != 0)
        {
            transform.localScale = new Vector3(horizontalDirection * 1f, 1f, 1);
        }
    }

    private void FixedUpdate()
    {
        if (hasAuthority)
        {
            //Move in response to input from WASD or Arrow Keys
            rb.velocity = movement.normalized * moveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void ReduceSpeed(float reduction)
    {
        moveSpeed = baseMoveSpeed * reduction;
        appliedReductionEffects += 1;
        StartCoroutine(RegainSpeed());
    }

    private IEnumerator RegainSpeed()
    {
        yield return new WaitForSeconds(3f);
        appliedReductionEffects -= 1;
        if (appliedReductionEffects == 0)
            moveSpeed = baseMoveSpeed;
        if (appliedReductionEffects < 0)
        {
            Debug.Log("Error: applied speed effects less than 0");
        }
    }
}
