using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Shoots projectile: used in throwing water balloon
public class Shoot2 : NetworkBehaviour
{
    public GameObject projectilePrefab; // prefab to be launched as projectile: must have projectile controller script
    public float projectileSpeed = 10f;   // speed of projectile
    public float shotCooldown = 1f; // cooldown before fireing another projectile

    [HideInInspector]
    public bool canShoot = true;
    [HideInInspector]
    public bool isShooting = false;

    private string projectileTag;

    void Start()
    {
        if (gameObject.tag == "PlayerOne")
            projectileTag = "PlayerOneProjectile";
        else if (gameObject.tag == "PlayerTwo")
            projectileTag = "PlayerTwoProjectile";
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Q)  && canShoot)
        {
            isShooting = true;
            StartCoroutine(ShotCooldown(shotCooldown));


            if (isServer)
            {
                RpcSpawnProjectile(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition),     // target
                    gameObject.transform.position,  // start location
                    gameObject.GetComponent<Rigidbody2D>().velocity,     // start speed
                    projectileTag  // launcher tag
                    );
            }
            else
            {
                CmdSpawnProjectile(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition),    // target
                    gameObject.transform.position,  // start location
                    gameObject.GetComponent<Rigidbody2D>().velocity,    // start speed
                    projectileTag  // launcher tag
                    );    
            }
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

/*    // Creates the projectile and sends it moving
    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, gameObject.transform.position, gameObject.transform.rotation);
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).parentTagName = gameObject.tag;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).startLocation = gameObject.transform.position;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).startSpeed = gameObject.GetComponent<Rigidbody2D>().velocity;
        *//*        (projectile.GetComponent("ProjectileController") as ProjectileController).speedReduction = speedReduction;*//* // Edit with special launcher??
    }*/

    [Command]
    private void CmdSpawnProjectile(Vector2 target, Vector2 startLocation, Vector2 startSpeed, string tag)
    {
        RpcSpawnProjectile(target, startLocation, startSpeed, tag);
    }

    // Creates a projectile and sends it moving
    [ClientRpc]
    private void RpcSpawnProjectile(Vector2 target, Vector2 startLocation, Vector2 startSpeed, string tag)
    {
        Vector2 dir = (Vector3)target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject projectile = Instantiate(projectilePrefab, startLocation, transform.rotation);
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).target = target;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).startLocation = startLocation;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).startSpeed = startSpeed;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).rotation = rotation;
        projectile.tag = tag;

    }
}
