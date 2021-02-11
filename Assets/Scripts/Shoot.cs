using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Shoots projectile: used in throwing water balloon
public class Shoot : MonoBehaviour
{
    public GameObject projectilePrefab; // prefab to be launched as projectile: must have projectile controller script
    public float projectileSpeed = 10f;   // speed of projectile
    public float shotCooldown = 1f; // cooldown before fireing another projectile

    [HideInInspector]
    public bool isLocalPlayer = false;
    [HideInInspector]
    public bool canShoot = true;
    [HideInInspector]
    public bool isShooting = false;

    void Start()
    {
        if (gameObject.tag == PlayerData.localPlayer.tag)
        {
            isLocalPlayer = true;
        }
    }

    void Update()
    {
        // Throw projectile key
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Q) && PlayerData.userArea.OverlapPoint(this.transform.position) && canShoot )
        {
            isShooting = true;
            StartCoroutine(ShotCooldown(shotCooldown));
            SpawnProjectile();
        }
        else if (isLocalPlayer)
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
        GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
        (projectile.GetComponent("ProjectileController") as ProjectileController).parentTagName = PlayerData.localPlayer.tag;      
/*        (projectile.GetComponent("ProjectileController") as ProjectileController).speedReduction = speedReduction;*/ // Edit with special launcher??
    }
}
