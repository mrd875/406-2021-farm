using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform shooter;
    //public Transform target;
    public GameObject projectilePrefab;
    
    public float projectileSpeed;
    public float shotCooldown = 1f;
    public bool canShoot = true;
    public float speedReduction = 2f;
    public bool isShooting = false;


    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.Find("Cursor").GetComponent<Transform>();
        //target = GameObject.Find("Player (1)").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
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

    private IEnumerator ShotCooldown(float cooldown)
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    private void SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, shooter.position, shooter.rotation);
        (projectile.GetComponent("ProjectileController") as ProjectileController).tagName = "PlayerOneProjectile";
        (projectile.GetComponent("ProjectileController") as ProjectileController).speedReduction = speedReduction;
        //(projectile.GetComponent("ProjectileController") as ProjectileController).target = target;
    }
}
