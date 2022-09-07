using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerVFXAnims
{
    Default,
    BasicShot_Charging,
    BasicShot_FullyCharged,
    ParryVFX



}


public class PlayerVFXController : MonoBehaviour
{
    PlayerMovement player;
    Animator animator;
    SpriteRenderer spriteRenderer;


    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    void Start()
    {
        spriteRenderer.enabled = false;
        animator.enabled = true;



    }

    // Update is called once per frame
    void Update()
    {
        
    }



    ///<summary>
    ///Plays a VFX animation according to a given enum. If condition is set to false,
    ///it will turn off visual components and reset timers.
    ///Can be given a transition animation and a duration to transition to a new animation
    ///after a set duration.
    ///</summary>
    public IEnumerator playVFXanim(bool condition,
    PlayerVFXAnims animEnum = PlayerVFXAnims.Default,
    PlayerVFXAnims transitionAnim = PlayerVFXAnims.Default, float duration = 0)
    {

        if(condition && animEnum != PlayerVFXAnims.Default)
        {
            animator.Play(animEnum.ToString());
            //animator.enabled = true;
            spriteRenderer.enabled = true;

            if(transitionAnim != PlayerVFXAnims.Default)
            {
                yield return new WaitForSecondsRealtime(duration);

                animator.Play(transitionAnim.ToString());
            }



        }else
        {
            spriteRenderer.enabled = false;
            animator.Play(PlayerVFXAnims.Default.ToString());
            //animator.Play(PlayerVFXAnims.Default.ToString());
            //animator.enabled = false;
            
        }

        yield break;

    }


}
