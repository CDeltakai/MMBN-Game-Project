using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowDownFactor = 0.5f;
    public float slowDownDuration = 3f;

    bool isCurrentlySlowedDown = false;

    void Update()
    {

        //while (Time.timeScale < 1 ){
        //Time.timeScale += (1f/slowDownDuration) * Time.unscaledDeltaTime;
        //}

    }


    public void SlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
        print("time manager used slowmotion");
        isCurrentlySlowedDown = true;
    }


    public void cancelSlowMotion()
    {
        isCurrentlySlowedDown = false;
        Time.timeScale = 1;
        print("time manager canceled slowmotion");
    }

}
