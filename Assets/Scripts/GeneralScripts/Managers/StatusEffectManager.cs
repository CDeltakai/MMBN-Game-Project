using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    BStageEntity entity;

    private void Awake() 
    {
        if(gameObject.GetComponent<BStageEntity>() != null)
        {
            entity = gameObject.GetComponent<BStageEntity>();
        }else
        {Debug.LogError("Game object does not contain a BStageEntity component. This component will be non-functional.");}

    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
