using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAround : MonoBehaviour
{
    public Animator animator;


    // basic movement variables
    public float moveSpeed = 5.0f;
    private Vector2 movement;

    private float degrees = 0;

    private bool isMoving = false;

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector2(1.0f, 0.0f);
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        StartCoroutine(ChangeDirection());

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

    }

    private IEnumerator ChangeDirection()
    {
        degrees += 90;
        if (degrees == 360)
            degrees = 0;

        movement.x = Mathf.Cos((degrees * Mathf.PI) / 180);
        movement.y = Mathf.Sin((degrees * Mathf.PI) / 180);
        yield return new WaitForSeconds(3f);
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        StartCoroutine(ChangeDirection());
    }
}
