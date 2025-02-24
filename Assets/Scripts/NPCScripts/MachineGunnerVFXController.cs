using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunnerVFXController : MonoBehaviour
{

    MachineGunner gunner;


    void Start()
    {
        gunner = FindObjectOfType<MachineGunner>();
    }

 

    public void gunnerHitRegister(int damage)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast (transform.position, new Vector2(-1, 0), 0.2f, LayerMask.GetMask("Player", "Player_Ally"));
        if(hitInfo)
        {
            hitInfo.collider.gameObject.SendMessage("HitByRay", SendMessageOptions.DontRequireReceiver);
            BStageEntity script = hitInfo.transform.gameObject.GetComponent<BStageEntity>();
            if(script != null)
            {
                script.hurtEntity(damage, true, true, gunner);
            }
        }
    }

}
