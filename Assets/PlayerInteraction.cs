using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode cutterKey = KeyCode.Q;
    public KeyCode itemKey = KeyCode.E;
    public Tilemap grassMap;
    public Tilemap dirtMap;

    public Tile dirt;
    public Tile grass;
    //public Tile actionTile;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerData.DropItem();
        }
        if (Input.GetKeyDown(cutterKey))
        {
            Vector3Int pos = grassMap.WorldToCell(gameObject.transform.position);
            if (grassMap.GetTile(pos) != null)
                grassMap.SetTile(pos, null);
        }
        if (Input.GetKeyDown(itemKey))
        {
            if (PlayerData.itemStack.Count > 0)
            {
                Vector3Int pos = grassMap.WorldToCell(gameObject.transform.position);
                if (PlayerData.itemStack.First.Value.actionTile == null)
                    Debug.Log("Error: No Item");
                if ((grassMap.GetTile(pos) == null) && (dirtMap.GetTile(pos) == dirt))
                {
                    grassMap.SetTile(pos, PlayerData.itemStack.First.Value.actionTile);
                    PlayerData.ItemUsed();
                }
            }
        }
        
    }

}
