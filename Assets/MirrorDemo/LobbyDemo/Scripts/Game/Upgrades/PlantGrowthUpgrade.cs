using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthUpgrade : MonoBehaviour
{

    public static void Activate()
    {
        PlayerData2.localGrowSpeed *= 0.7f;
    }
}
