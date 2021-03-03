using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller used for individual projectiles
public class ProjectileController2 : MonoBehaviour
{
    public Vector2 target; // where the projectile should move to
    public Vector2 startLocation; // where the projectile is launched
    public Vector2 startSpeed;  // The speed of the launcher slightly affects the speed of the projectile
    public float speed = 10;    // Speed of the projectile

    public GameObject shovelPrefab;

    public float lifeTime = 0.5f;   // how long the projectile will remain airborn for
    public float speedReduction = 0.5f; // default reduction in movement speed to target hit

    [HideInInspector]
    public string parentTagName; // must be set after instantiation

    private Vector2 direction; // Direction for projectile to move

    // Start is called before the first frame update
    void Start()
    {
        direction = (target - startLocation).normalized;
      
        // bullet is destroyed after a given time after not colliding with anything
        Destroy(this.gameObject, lifeTime);
    }

    private void Update()
    {
        // Object moves slightly faster in the direction it is thrown
        transform.Translate((direction * speed + (startSpeed / 2)) * Time.deltaTime);
        if (gameObject.tag == "Untagged")
        {
            if (parentTagName == "PlayerOne") { gameObject.tag = "PlayerOneProjectile"; }
            else if (parentTagName == "PlayerTwo") { gameObject.tag = "PlayerTwoProjectile"; }
            else if (parentTagName == "PlayerThree") { gameObject.tag = "PlayerThreeProjectile"; }
            else if (parentTagName == "PlayerFour") { gameObject.tag = "PlayerFourProjectile"; }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Player one projectile hits player two
        if (gameObject.tag == "PlayerOneProjectile" && (other.tag == "PlayerTwo" || other.tag == "PlayerThree" || other.tag == "PlayerFour"))
        {
            // Don't slow other player if they are in their own field
            if (!other.gameObject.GetComponent<PlayerTouch>().inHomeZone)
                other.GetComponent<PlayerMovement2>().ReduceSpeed(speedReduction);
            Destroy(this.gameObject);

        }
        else if (gameObject.tag == "PlayerTwoProjectile" && (other.tag == "PlayerOne" || other.tag == "PlayerThree" || other.tag == "PlayerFour"))
        {
            // Don't slow other player if they are in their own field
            if (other.gameObject.GetComponent<PlayerTouch>().inHomeZone)
                other.GetComponent<PlayerMovement2>().ReduceSpeed(speedReduction);
            Destroy(this.gameObject);
        }

        // Projectile should not be destroyed when collision occurs with the one who launches it
        else if (gameObject.tag == "PlayerOneProjectile" && other.tag == "PlayerOne") { }
        else if (gameObject.tag == "PlayerTwoProjectile" && other.tag == "PlayerTwo") { }

        else
        {
            // Destroy(this.gameObject);
        }
    }


}
