using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationController : MonoBehaviour
{
    Animator animator;
    [field:SerializeField] public AnimationClip IdleAnimation {get; protected set;}
    [field:SerializeField] public AnimationClip DefeatAnimation{get; protected set;}
    
    [SerializeField] protected List<AnimationClip> animationList;

    //Bool for checking if an animation is already playing (that is not the idle animation)
    public bool isAnimating;

    void Awake()
    {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    //Plays an animation based on the given animation clip's name, then returns to idle animation after the animation clip's
    //length. 
    public void PlayAnimationClip(AnimationClip animationClip)
    {
        

        animator.Play(animationClip.name);
        isAnimating = true;
        StartCoroutine(ReturnToIdle(animationClip.length));
    }

    //Plays an animation clip that ignores the isAnimating bool and does not return to idle. Unless the animation loops,
    //the object will stay on the last frame of the animation until another animation is played.
    public void PlayOneShotAnimation(AnimationClip animationClip)
    {
        animator.Play(animationClip.name);   
    }

    //Waits the given duration in seconds and then plays the predefined idle animation clip
    IEnumerator ReturnToIdle(float duration)
    {

        yield return new WaitForSeconds(duration);
        isAnimating = false;
        animator.Play(IdleAnimation.name);

    }
}
