using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement2 : NetworkBehaviour
{
    // basic movement variables
    public float maxMoveSpeed = 200.0f;
    private float activeMoveSpeed;
    public float speedUpgradeMagnitude = 1.2f;
    public int speedUpgradeCount = 0;
    private bool isMoving = false;
    private int appliedReductionEffects; // amount of slow effects applied (for duration purpose)
    public Vector3 movement;
    public Animator animator;


    // Player Rigidbody component to add movement to
    [HideInInspector]
    public Rigidbody2D rb;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        activeMoveSpeed = maxMoveSpeed;
    }


    void Update()
    {
        if (hasAuthority)
        {
            // Get input for movement from WASD or Arrow keys
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);

            if (movement != new Vector3(0, 0) && isMoving == false)
            {
                SoundControl.PlayMoveSound();
                isMoving = true;
            }
            else if (movement == new Vector3(0, 0) && isMoving == true)
            {
                SoundControl.StopMoveSound();
                isMoving = false;
            }
        }
    }


    private void FixedUpdate()
    {
        if (hasAuthority)
        {
            //Move in response to input from WASD or Arrow Keys
            rb.velocity = movement.normalized * activeMoveSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }


    public void SpeedUpgrade()
    {
        maxMoveSpeed *= speedUpgradeMagnitude;
        activeMoveSpeed = maxMoveSpeed;
        speedUpgradeCount += 1;
    }


    // Effects that reduce target movement speeds call this function
    public void ReduceSpeed(float reduction)
    {
        if (isServer)
            RpcReduceSpeed(reduction);
        else
            CmdReduceSpeed(reduction);
    }

    // Changes movement speed back to normal after a certain time. Timer resets when hit by another projectile.
    private IEnumerator RegainSpeed()
    {
        StatusEffects icon = transform.GetChild(0).GetComponent<StatusEffects>();
        Debug.Log(gameObject.tag);
        icon.StartEffect("Wet");
        yield return new WaitForSeconds(3f);
        icon.StopEffects();
        appliedReductionEffects -= 1;
        if (appliedReductionEffects == 0)
            activeMoveSpeed = maxMoveSpeed;
        if (appliedReductionEffects < 0)
        {
            Debug.Log("Error: applied speed effects less than 0");
        }
    }

    public IEnumerator Trapped(GameObject trap, float trapTime)
    {
        StatusEffects statusIcon = transform.GetChild(0).GetComponent<StatusEffects>();
        statusIcon.StartEffect("Bear Trap");
        if (hasAuthority)
        {
            PlayerData2.playerClick.enabled = false;
            PlayerData2.playerShoot.canShoot = false;
        }
        activeMoveSpeed = 0.0f;
        yield return new WaitForSeconds(trapTime);
        if (hasAuthority)
        {
            
            PlayerData2.playerClick.enabled = true;
            PlayerData2.playerShoot.canShoot = true;
        }
        statusIcon.StopEffects();
        activeMoveSpeed = maxMoveSpeed;
        Destroy(trap);
    }

    [Command]
    private void CmdReduceSpeed(float reduction)
    {
        RpcReduceSpeed(reduction);
    }

    [ClientRpc]
    private void RpcReduceSpeed(float reduction)
    {
        activeMoveSpeed = maxMoveSpeed * reduction;
        appliedReductionEffects += 1;
        StartCoroutine(RegainSpeed());
    }
}
