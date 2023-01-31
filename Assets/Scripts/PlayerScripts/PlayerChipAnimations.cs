using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerChipAnimations : MonoBehaviour
{
 

    float currentAnimationLength;
    private string currentState;
    bool isInAnimation = false;

    Animator animator;

    private void Awake() 
    {
        animator = GetComponent<Animator>();

        
    }



    public void PlayAnimationClip(AnimationClip animation)
    {
        ChangeAnimationState(animation.name);
        print("Animation played: " + animation.name);
        StartCoroutine(ReturnToIdle(animation.length));
    }

    public void PlayAnimationOneShotString(string animationName, float duration)
    {
        ChangeAnimationState(animationName);
        StartCoroutine(ReturnToIdle(duration));

    }


    IEnumerator ReturnToIdle(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        ChangeAnimationState("Megaman_Idle");
    }

    void ChangeAnimationState(string newState)
    {
        if(currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
        
    }


}
