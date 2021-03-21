using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public string trapOwnerTag = "PlayerTwo";
    public float trapTime = 2.5f;
    public float visibilityDistance = 2.0f;
    private bool triggered = false;
    

    void Start()
    {
        if (trapOwnerTag == PlayerData2.localPlayer.tag)
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        else
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if (trapOwnerTag != PlayerData2.localPlayer.tag)
        {
            if (Vector2.Distance(PlayerData2.localPlayer.transform.position, transform.position) < visibilityDistance)
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
            return;
        if (other.tag == "PlayerOne" || other.tag == "PlayerTwo")
        {
            if (other.tag != trapOwnerTag)
            {
                triggered = true;
                other.transform.position = gameObject.transform.position + new Vector3(0, 0.7f, 0);
                PlayerMovement2 playerMovement = other.GetComponent<PlayerMovement2>();
                StartCoroutine(playerMovement.Trapped(this.gameObject, trapTime));
            }
        }
    }

/*    private IEnumerator Trapped(PlayerMovement2 playerMovement)
    {
        float baseMovementSpeed = playerMovement.baseMoveSpeed;
        playerMovement.moveSpeed = 0.0f;
        yield return new WaitForSeconds(trapTime);
        playerMovement.moveSpeed = baseMovementSpeed;
        Destroy(this.gameObject);
    }*/
}
