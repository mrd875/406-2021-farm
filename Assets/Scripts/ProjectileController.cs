using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller used for individual projectiles
public class ProjectileController : MonoBehaviour
{
    public Vector2 target; // where the projectile should move to
    public Vector2 startLocation; // where the projectile is launched
    public Vector2 startSpeed;  // The speed of the launcher slightly affects the speed of the projectile
    public float speed = 10;    // Speed of the projectile

    public float lifeTime = 0.5f;   // how long the projectile will remain airborn for
    public float speedReduction = 10; // default reduction in movement speed to target hit
    public string tagName = "PlayerOneProjectile"; // default tag, for interaction purpose so, for example, player one cannot get hit by their own projectile

    private Vector2 direction; // Direction for projectile to move

    // Start is called before the first frame update
    void Start()
    {
        startLocation = PlayerData.player.transform.position;
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        startSpeed = PlayerData.playerRb.velocity;

        transform.gameObject.tag = tagName;
        direction = (target - startLocation).normalized;

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
        // Player one projectile hits player two
        if (tagName == "PlayerOneProjectile" && other.tag == "PlayerTwo")
        {
            other.GetComponent<MoveAround>().moveSpeed /= 2;
            Destroy(this.gameObject);
        }
    }


}
