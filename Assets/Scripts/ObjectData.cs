using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public GameObject setShovelPrefab;
    public GameObject setBearTrapItemPrefab;
    public GameObject setBearTrapPrefab;

    public static GameObject shovelPrefab;
    public static GameObject bearTrapItemPrefab;
    public static GameObject bearTrapPrefab;

    void Start()
    {
        shovelPrefab = setShovelPrefab;
        bearTrapItemPrefab = setBearTrapItemPrefab;
        bearTrapPrefab = setBearTrapPrefab;
    }
}
