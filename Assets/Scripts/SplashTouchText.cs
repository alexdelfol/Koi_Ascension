using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashTouchText : MonoBehaviour
{
    private Color color;
    private bool fade;
    private float timeF;
    private float alphaFade;
    private float loopTime;
    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<Text>().color;
        loopTime = 1.75f;
        fade = true;
        alphaFade = 1;
        timeF = loopTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(fade)
        {
            if(timeF < 0)
            {
                fade = false;
                alphaFade = 0;
                timeF = loopTime;

            } else
            {
                gameObject.GetComponent<Text>().color = new Color(color.r, color.g, color.b, alphaFade);
                timeF -= Time.deltaTime;
                alphaFade = timeF/loopTime;
            }
        }

        else
        {
            if (timeF < 0)
            {
                fade = true;
                alphaFade = 1;
                timeF = loopTime;

            }
            else
            {
                gameObject.GetComponent<Text>().color = new Color(color.r, color.g, color.b, alphaFade);
                timeF -= Time.deltaTime;
                alphaFade = 1 - (timeF/loopTime);
            }
        }
    }
}
