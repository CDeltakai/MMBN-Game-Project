using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericVFXController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    Coroutine animationCoroutine;


    void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();


    }

    private void Start()
    {
        spriteRenderer.enabled = false;


    }

    public void TriggerVFX(string stateName, float duration, bool realtime = false)
    {
        if(animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(PlayVFX(stateName, duration, realtime));
        }else
        {
            animationCoroutine = StartCoroutine(PlayVFX(stateName, duration, realtime));
        }
    }

    public IEnumerator PlayVFX(string stateName, float duration, bool realtime = false)
    {
        spriteRenderer.enabled = true;
        animator.Play(stateName);



        if(!realtime)
        {
            yield return new WaitForSeconds(duration);
        }else
        {
            yield return new WaitForSecondsRealtime(duration);
        }

        animator.Play("Default");
        spriteRenderer.enabled = false;

    }

    public void DisableVFX()
    {
        StopCoroutine(animationCoroutine);
        animator.Play("Default");
        spriteRenderer.enabled = false;
    }



}
