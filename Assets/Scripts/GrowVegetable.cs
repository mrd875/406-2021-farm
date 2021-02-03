using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowVegetable : MonoBehaviour
{

    //Public inspector variables
    [System.Serializable]
    public struct stages
    {
        public Sprite image;
        public float growTime;
    }
    public List<stages> vegetableStages;


    //Internal
    private int currentStageIndex = 0;
    private int maxStages;
    private float growTimer;
    private SpriteRenderer spriteController;
    private stages currentStage;

    // Start is called before the first frame update
    void Start()
    {
        //Get starting variables
        spriteController = GetComponent<SpriteRenderer>();
        maxStages = vegetableStages.Count;
        
        //Set vegetable and vegetable parameters to first vegetable
        currentStage = vegetableStages[currentStageIndex];
        growTimer = currentStage.growTime;
        spriteController.sprite = currentStage.image;


        StartCoroutine(VegetableTimer());
    }


    private IEnumerator VegetableTimer()
    {
        //While there are more vegetable stages....
        while (currentStageIndex < maxStages - 1)
        {
            //Wait for stage timer
            yield return new WaitForSeconds(growTimer);

            //Advance to next vegetable
            currentStageIndex += 1;
            currentStage = vegetableStages[currentStageIndex];
            
            spriteController.sprite = currentStage.image;
            growTimer = currentStage.growTime;
        }
    }

}
