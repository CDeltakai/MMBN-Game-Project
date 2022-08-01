using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShot : MonoBehaviour
{

    public Transform firePoint;
    int layerMask = 9;
    
    PlayerMovement player;

    void Start() 
    {
      player = FindObjectOfType<PlayerMovement>();

    }


    public void Shoot()
    {
      RaycastHit2D hitInfo = Physics2D.Raycast (firePoint.position, firePoint.right, Mathf.Infinity, LayerMask.GetMask("Enemies","Obstacle"));

      if(hitInfo)
      {

          //DamageFunctions script = hitInfo.transform.gameObject.GetComponent<DamageFunctions>();
          IBattleStageEntity target = hitInfo.transform.gameObject.GetComponent<IBattleStageEntity>();
          if(target == null)
          {return;}
          target.hurtEntity(player.basicShotDamage(), true, false);
          //script.hurtEntity(player.basicShotDamage());

          
      }
    }
}
