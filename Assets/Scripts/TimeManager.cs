using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static float slowDownFactor = 0.15f;
    public static float slowDownDuration = 3f;

    public static bool isCurrentlySlowedDown = false;

    public static Coroutine SlowMoCoroutine{get; private set;} = null;

    void Update()
    {

        //while (Time.timeScale < 1 ){
        //Time.timeScale += (1f/slowDownDuration) * Time.unscaledDeltaTime;
        //}

    }


    public void SlowMotion(float slowDownFactor = 0.15f)
    {
        Time.timeScale = slowDownFactor;
        print("time manager used slowmotion");
        isCurrentlySlowedDown = true;
        GameManager.BlockPausing = true;

    }


    public void CancelSlowMotion()
    {
        isCurrentlySlowedDown = false;
        GameManager.BlockPausing = false;
        Time.timeScale = 1;
        print("time manager canceled slowmotion");
    }


    public void TriggerSlowMotion(float slowFactor, float duration)
    {
        if(SlowMoCoroutine != null)
        {
            print("Slow motion has already been triggered!");
            return;
        }

        SlowMoCoroutine = StartCoroutine(SlowMotionCoroutine(slowFactor, duration));
        
    }


    IEnumerator SlowMotionCoroutine(float slowFactor, float duration)
    {
        SlowMotion(slowFactor);
        yield return new WaitForSecondsRealtime(duration);
        CancelSlowMotion();
        SlowMoCoroutine = null;
    }



}
