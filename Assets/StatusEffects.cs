using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [System.Serializable]
    public struct statusEffect
    {
        public string name;
        public GameObject prefab;
    }

    public statusEffect[] effects;
    // Start is called before the first frame update

    public void StartEffect(string effectName)
    {
        foreach (var statusEffect in effects)
        {
            if (statusEffect.name == effectName)
            {
                GameObject icon = Instantiate(statusEffect.prefab, this.transform);
                icon.transform.localPosition = Vector3.zero;
            }
        }
    }

    public void StopEffects()
    {
        int childrenCount = transform.childCount;

        for (int i = 0; i < childrenCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
    }
}
