using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    [SerializeField] BStageEntity entity;
    Animator entityAnimator;
    [SerializeField] EntityVFXManager EntityVFXController;


    Dictionary<Coroutine,EStatusEffects> statusEffectCoroutines;

    public float ParalyzeDefaultDuration = 1.25f;
    
    Coroutine ParalyzeCoroutine;

    public float FrozenDefaultDuration = 1.5f;
    Coroutine FrozenCoroutine;

    Coroutine RootedCoroutine;

    public float MarkForDeathDefaultDuration = 3f;
    Coroutine MarkForDeathCoroutine;


    private void Awake() 
    {
        if(gameObject.GetComponent<BStageEntity>() != null)
        {
            entity = gameObject.GetComponent<BStageEntity>();
        }else
        {
            Debug.LogError("Game object does not contain a BStageEntity component. This component will be non-functional.");
            return;
        }

        entityAnimator = entity.gameObject.GetComponent<Animator>();

    }


    void Start()
    {
        
    }

    //if toggle = true, will try to trigger the status effect, otherwise will attempt to cancel that effect on the entity.
    public void triggerStatusEffect(EStatusEffects status, float duration = 1, bool toggle = true, int strength = 0)
    {
        print("Attempted to set statusEffect: " + status + " on target: " +  this.gameObject.name);
        StartCoroutine(setStatusEffect(status, duration, toggle, strength));
    }

    public IEnumerator setStatusEffect(EStatusEffects status, float duration = 1, bool toggle = true, int strength = 0)
    {
        switch(status)
        {
            case EStatusEffects.Default:
            break;
            
            case EStatusEffects.Paralyzed:
                        
                if(!entity.isStunnable){yield break;}


                if(ParalyzeCoroutine != null)
                {
                    StopCoroutine(ParalyzeCoroutine);
                    ParalyzeCoroutine = StartCoroutine(SetParalyzed(ParalyzeDefaultDuration));
                }else
                {
                    ParalyzeCoroutine = StartCoroutine(SetParalyzed(ParalyzeDefaultDuration));
                }

            break;

            case EStatusEffects.Rooted:
                if(RootedCoroutine != null)
                {
                    StopCoroutine(RootedCoroutine);
                    RootedCoroutine = StartCoroutine(SetRooted(duration));
                }else
                {
                    RootedCoroutine = StartCoroutine(SetRooted(duration));
                }
            break;

            case EStatusEffects.Frozen:
                if(!entity.isStunnable){yield break;}


            break;

            case EStatusEffects.Bleeding:
            print("Triggered Bleeding Effect");

            entity.DamageEntity(10, 0.25f, 2f);

            break;

            case EStatusEffects.MarkForDeath:
                print("Triggered MarkForDeath");

                if(!toggle)
                {
                    StopCoroutine(MarkForDeathCoroutine);
                    MarkForDeathCoroutine = StartCoroutine(SetMarkForDeath(0, false));
                }

                if(MarkForDeathCoroutine != null)
                {
                    StopCoroutine(MarkForDeathCoroutine);
                    MarkForDeathCoroutine = StartCoroutine(SetMarkForDeath(MarkForDeathDefaultDuration));
                }else
                {
                    MarkForDeathCoroutine = StartCoroutine(SetMarkForDeath(MarkForDeathDefaultDuration));
                }

            break;

            

            default:
                Debug.LogWarning("The given status effect:  " +  status + " does not exist or has not been implemented, aborted setting status effect.");
            break;





        }


        yield break;

    }


    IEnumerator SetParalyzed(float duration)
    {


            //if(entity.nonVolatileStatus){yield break;}

            entity.isStunned = true;
            //entity.nonVolatileStatus = true;

            entityAnimator.speed = 0f;
            StartCoroutine(entity.FlashColor(Color.yellow, duration));
            yield return new WaitForSeconds(duration);

            entityAnimator.speed = 1f;
            //entity.nonVolatileStatus = false;

    }


    IEnumerator SetRooted(float duration)
    {
        entity.isRooted = true;
        yield return new WaitForSeconds(duration);
        entity.isRooted = false;

    }

    void SetFrozen(float duration)
    {


    }

    IEnumerator SetMarkForDeath(float duration = 3, bool toggle = true)
    {
        print("SetMarkForDeath triggered");

        if(!toggle)
        {
            entity.MarkedForDeath = false;
            EntityVFXController.DisableVFX(EStatusEffects.MarkForDeath);
            yield break;
        }


        entity.MarkedForDeath = true;
        EntityVFXController.PlayStatusVFX(EStatusEffects.MarkForDeath, duration);
        yield return new WaitForSeconds(duration);
        entity.MarkedForDeath = false;

        yield break;
    }



}
