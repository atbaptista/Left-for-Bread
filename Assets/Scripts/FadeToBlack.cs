using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    public GameObject BlackOutSquare;
    public bool DoAtStart = true;

    public int FadeSpeed = 5;

    public void Start()
    {
        if (DoAtStart)
        {
            //fade from black to scene at the start of the scene.
            StartFadeout(false);
        }
    }


    public void StartFadeout(bool fadeToBlack)
    {
        StartCoroutine(FadeBlackOutSquare(fadeToBlack));
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack)
    {
        //yield return new WaitForEndOfFrame();
        Color objectColor = BlackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (BlackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (FadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (BlackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (FadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                BlackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }
}
