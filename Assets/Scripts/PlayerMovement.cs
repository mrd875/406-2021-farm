using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // basic movement variables
    public float moveSpeed = 200.0f;
    private float baseMoveSpeed;
    private bool isMoving = false;
    private int appliedReductionEffects; // amount of slow effects applied (for duration purpose)
    public Vector2 movement;
    

    // Player RB to add movement to
    public Rigidbody2D rb;

    void Start()
    {
        baseMoveSpeed = moveSpeed;
    }

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
        rb.velocity = movement.normalized * moveSpeed * Time.fixedDeltaTime;
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
            Debug.Log("Error, applied speed effects less than 0");
        }
    }
}
