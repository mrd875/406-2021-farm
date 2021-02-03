using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector2 target;
    public Vector2 startLocation;
    public Vector2 startSpeed;
    public float speed = 10;

    public float lifeTime = 0.5f;
    public float speedReduction = 10; // default reduction
    public string tagName = "PlayerOneProjectile"; // default tag

    private Rigidbody2D rb;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        startLocation = PlayerData.player.transform.position;
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startSpeed = PlayerData.playerRb.velocity;


        transform.gameObject.tag = tagName;
        direction = (target - startLocation).normalized;
        //Debug.Log(dy);
        //destroy after a set time in case it doesn't collide with anything
        Destroy(this.gameObject, lifeTime);
    }

    private void Update()
    {
        // Object moves slightly faster in the direction it is thrown
        transform.Translate((direction * speed + (startSpeed/2)) * Time.deltaTime); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (tagName == "PlayerOneProjectile" && other.tag == "PlayerTwo")
        {
            other.GetComponent<MoveAround>().moveSpeed /= 2;
            Destroy(this.gameObject);
        }
    }


}
