using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    // The time it takes for the camera to reach the player
    public float dampTime = 0.4f;
    private Vector3 cameraPos;
    private Vector3 velocity = Vector3.zero;

    [HideInInspector]
    GameSpawner gs;

    // Get player location reference;
    [HideInInspector]
    public Transform player;

    private void Start()
    {
        gs = GameObject.FindGameObjectWithTag("GameSpawner").GetComponent<GameSpawner>();
    }

    void Update()
    {
        if (gs.localPlayer == null)
            return;

        player = gs.localPlayer.transform;

        // Make new Vector in direction of the player
        cameraPos = new Vector3(player.position.x, player.position.y, -10f);

        // Update the position of the camera
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraPos, ref velocity, dampTime);
    }
}
