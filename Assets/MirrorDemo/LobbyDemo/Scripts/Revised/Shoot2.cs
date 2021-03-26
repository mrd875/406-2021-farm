using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Shoots projectile: used in throwing water balloon
public class Shoot2 : NetworkBehaviour
{
    public GameObject projectilePrefab; // prefab to be launched as projectile: must have projectile controller script
    public float projectileSpeed = 10f;   // speed of projectile
    public float shotCooldown = 3f; // cooldown before fireing another projectile
    public float cooldownProgress;

    [HideInInspector]
    public bool canShoot = true;
    [HideInInspector]
    public bool isShooting = false;

    private string projectileTag;

    void Start()
    {
        cooldownProgress = shotCooldown;

        if (gameObject.tag == "PlayerOne")
            projectileTag = "PlayerOneProjectile";
        else if (gameObject.tag == "PlayerTwo")
            projectileTag = "PlayerTwoProjectile";
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (cooldownProgress <= shotCooldown)
        {
            cooldownProgress += Time.deltaTime;
            if (cooldownProgress >= shotCooldown)
                canShoot = true;
        }

        if (canShoot && Input.GetMouseButton(1))
        {
            isShooting = true;
            canShoot = false;
            cooldownProgress = 0.0f;

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

    [Command]
    private void CmdSpawnProjectile(Vector2 target, Vector2 startLocation, Vector2 startSpeed, string tag)
    {
        RpcSpawnProjectile(target, startLocation, startSpeed, tag);
    }

    // Creates a projectile and sends it moving
    [ClientRpc]
    private void RpcSpawnProjectile(Vector2 target, Vector2 startLocation, Vector2 startSpeed, string tag)
    {

        GameObject projectile = Instantiate(projectilePrefab, startLocation, transform.rotation);
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).target = target;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).startLocation = startLocation;
        (projectile.GetComponent("ProjectileController2") as ProjectileController2).startSpeed = startSpeed;
        projectile.tag = tag;

    }
}
