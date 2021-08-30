using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : Singleton<FadeInOut>
{
    public Image fadeImg;

    public bool isInTransition;
    public float transition;
    public bool isShowing;
    // Fade In 처리 시간
    [Range(0.01f, 5.0f)]
    public float fadeDuration = 0.001f;


    public void Fade(bool showing, float duration)
    {
        isShowing = showing;
        isInTransition = true;
        this.fadeDuration = duration;
        transition = (isShowing) ? 0 : 1;
    }

    private void Update()
    {        
        if (!isInTransition)
            return;

        transition += (isShowing) ? Time.deltaTime * (1 / fadeDuration) : -Time.deltaTime * (1 / fadeDuration);
        fadeImg.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);

        if(transition >1 || transition < 0)
        {
            isInTransition = false;
            Player.Instance.moveBool = true;
            
        }
        else
        {
            Player.Instance.moveBool = false;
        }
    }
}
