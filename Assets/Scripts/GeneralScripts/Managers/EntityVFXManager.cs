using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityVFXManager : MonoBehaviour
{

    [SerializeField] GameObject[] GenericVFXControllers = new GameObject[7];
    [SerializeField] List<GameObject> EntityVFXControllers = new List<GameObject>();
    public Dictionary<EStatusEffects, GameObject> GenericVFXControllerStates = new Dictionary<EStatusEffects, GameObject>();


    void Awake()
    {
 
    }

    private void Start() 
    {

        foreach(GameObject VFXController in GenericVFXControllers)
        {
            VFXController.GetComponent<SpriteRenderer>().enabled = false;
        }


        GenericVFXControllerStates.Add(EStatusEffects.Paralyzed, GenericVFXControllers[0]);
        GenericVFXControllerStates.Add(EStatusEffects.Rooted, GenericVFXControllers[1]);
        GenericVFXControllerStates.Add(EStatusEffects.Frozen, GenericVFXControllers[2]);
        GenericVFXControllerStates.Add(EStatusEffects.Burning, GenericVFXControllers[3]);
        GenericVFXControllerStates.Add(EStatusEffects.Poison, GenericVFXControllers[4]);
        GenericVFXControllerStates.Add(EStatusEffects.MarkForDeath, GenericVFXControllers[5]);
        GenericVFXControllerStates.Add(EStatusEffects.Bleeding, GenericVFXControllers[6]);

    }

    public void PlayStatusVFX(EStatusEffects statusEffect, float duration)
    {

        GameObject VFXGameObject = GenericVFXControllerStates[statusEffect];
        GenericVFXController vfxController = VFXGameObject.GetComponent<GenericVFXController>();

        vfxController.TriggerVFX(statusEffect.ToString(), duration);


    }

    public void DisableVFX(EStatusEffects statusEffect)
    {
        GameObject VFXGameObject = GenericVFXControllerStates[statusEffect];
        GenericVFXController vfxController = VFXGameObject.GetComponent<GenericVFXController>();

        vfxController.DisableVFX();        
    }


    public void PlayGenericVFX()
    {

    }


    // public IEnumerator PlayStatusVFX(EStatusEffects statusEffect, float duration)
    // {

    //     for(int i = 0; i < VFXControllers.Count() ; i++) 
    //     {
    //         if(VFXControllerStates[VFXControllers[i]] == false)
    //         {
    //             VFXControllerStates[VFXControllers[i]] = true;
    //             VFXControllers[i].GetComponent<SpriteRenderer>().enabled = true;
    //             VFXControllers[i].GetComponent<Animator>().Play(statusEffect.ToString());

    //             yield return new WaitForSeconds(duration);

    //             VFXControllers[i].GetComponent<SpriteRenderer>().enabled = false;
    //             VFXControllers[i].GetComponent<Animator>().Play("Default");
    //             VFXControllerStates[VFXControllers[i]] = false;

    //             yield break;
    //         }

    //     }

    //     Debug.LogWarning("EntityVFXController ran out of VFXController slots, could not play the specfied status VFX: " + statusEffect.ToString() +
    //     ". Are there too many different unique status effects coming in at once?");


    // }







 


}
