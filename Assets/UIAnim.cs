using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class UIAnim : MonoBehaviour
{
    public Sprite[] frames;
    public float frameDelay;

    private int frameCount;
    private int currentFrame = 0;
    private UnityEngine.UI.Image imageComp;
    private bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        frameCount = frames.Length;
        imageComp = GetComponent<Image>();

    }

    public void StartAnim()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            imageComp.color = Color.white;
            StartCoroutine(PlayAnim());
        }

    }

    private IEnumerator PlayAnim()
    {
        while (currentFrame < frameCount)
        {
            imageComp.sprite = frames[currentFrame];
            yield return new WaitForSeconds(frameDelay);

            currentFrame += 1;

        }

        isPlaying = false;
        imageComp.color = Color.clear;
        currentFrame = 0;

    }

}
