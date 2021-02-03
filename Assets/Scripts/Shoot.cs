using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shoots projectile: used in throwing water balloon
public class Shoot : MonoBehaviour
{
    
    public Transform shooter;   // Player who shot the projectile
    public GameObject projectilePrefab; // prefab to be launched as projectile: must have projectile controller script
    
    public float projectileSpeed;   // speed of projectile
    public float shotCooldown = 1f; // cooldown before fireing another projectile
    public float speedReduction = 2f;   // Amount of movement speed that is reduced when projecitile collides with other player (NYI)

    public bool canShoot = true;
    public bool isShooting = false;

    void Update()
    {
        // Throw projectile key
        if (Input.GetKeyDown(KeyCode.Q) && canShoot)
        {
            isShooting = true;
            StartCoroutine(ShotCooldown(shotCooldown));
            SpawnProjectile();
        }
        else
        {
            isShooting = false;
        }
    }

    // Activates shot cooldown, a period which no shots can be fired in
    private IEnumerator ShotCooldown(float cooldown)
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    // Creates the projectile and sends it moving
    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, shooter.position, shooter.rotation);
        (projectile.GetComponent("ProjectileController") as ProjectileController).tagName = "PlayerOneProjectile";
        (projectile.GetComponent("ProjectileController") as ProjectileController).speedReduction = speedReduction;
        //(projectile.GetComponent("ProjectileController") as ProjectileController).target = target;
    }
}
