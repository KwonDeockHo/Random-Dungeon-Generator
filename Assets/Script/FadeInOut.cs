using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : Singleton<FadeInOut>
{
    public Image fadeBackground;
    public float fadeChange_value;
    public bool isShowInOut;
    public bool isFadeChanging;

    // Fade In 처리 시간
    [Range(0.01f, 5.0f)]
    public float fadeDurationUnit = 0.001f;

    public void setFade(bool showing, float duration)
    {
        isShowInOut = showing;
        isFadeChanging = true;
        fadeDurationUnit = duration;
        fadeChange_value = (isShowInOut) ? 0 : 1;
    }

    private void Update()
    {        
        if (!isFadeChanging)
            return;

        fadeChange_value += (isShowInOut) ? Time.deltaTime * (1 / fadeDurationUnit) : -Time.deltaTime * (1 / fadeDurationUnit);
        fadeBackground.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, fadeChange_value);

        if(fadeChange_value > 1 || fadeChange_value < 0)
        {
            isFadeChanging = false;
            Player.Instance.isMoveStatus = true;
            
        }
        else
        {
            Player.Instance.isMoveStatus = false;
        }
    }
}
