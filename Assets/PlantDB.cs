using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDB : MonoBehaviour
{
    //Pushes plant information to WorldData2 so that all clients can
    //use all vegetables

    [Serializable]
    public struct plantInfo
    {
        public GameObject plantObject;
        public string plantName;
    }

    public List<plantInfo> plants;
    
    //On start, push all information up to WorldData2
    void Start()
    {
        foreach (plantInfo plant in plants)
        {
            WorldData2.CreateAndAddPlant(plant.plantName, plant.plantObject);
        }
    }

}
