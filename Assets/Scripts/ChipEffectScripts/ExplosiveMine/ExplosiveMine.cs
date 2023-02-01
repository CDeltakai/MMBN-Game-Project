using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ExplosiveMine : ObjectSummonAttributes
{
    [SerializeField] BattleStageHandler stageHandler;
    [SerializeField] GameObject MineObject;
    [SerializeField] EventReference MineActivateSFX;
    [SerializeField] EventReference NormalExplosionSFX;
    [SerializeField] EventReference BigExplosionSFX;

    [SerializeField] GameObject PrimaryExplosion;
    [SerializeField] GameObject NorthExplosion;
    [SerializeField] GameObject SouthExplosion;
    [SerializeField] GameObject EastExplosion;
    [SerializeField] GameObject WestExplosion;

    GameObject primaryExplosionObject;
    GameObject northExplosionObject;
    GameObject southExplosionObject;
    GameObject eastExplosionObject;
    GameObject westExplosionObject;

    SecondaryExplosions primaryExplosionScript;
    SecondaryExplosions northExplosionScript;
    SecondaryExplosions southExplosionScript;
    SecondaryExplosions eastExplosionScript;
    SecondaryExplosions westExplosionScript;



    Animator MineObjectAnimator;

    SpriteRenderer MineObjectSprRenderer;

    BoxCollider2D MineObjectBoxCollider2D;

    private void Awake() 
    {
        MineObjectAnimator = MineObject.GetComponent<Animator>();
        MineObjectSprRenderer = MineObject.GetComponent<SpriteRenderer>();
        MineObjectBoxCollider2D = MineObject.GetComponent<BoxCollider2D>();
        stageHandler = BattleStageHandler.Instance;

        primaryExplosionScript = PrimaryExplosion.GetComponentInChildren<SecondaryExplosions>();
        northExplosionScript = NorthExplosion.GetComponentInChildren<SecondaryExplosions>();
        southExplosionScript = SouthExplosion.GetComponentInChildren<SecondaryExplosions>();
        eastExplosionScript = EastExplosion.GetComponentInChildren<SecondaryExplosions>();
        westExplosionScript = WestExplosion.GetComponentInChildren<SecondaryExplosions>();


        primaryExplosionObject = PrimaryExplosion.transform.GetChild(0).gameObject;
        northExplosionObject = NorthExplosion.transform.GetChild(0).gameObject;
        southExplosionObject = SouthExplosion.transform.GetChild(0).gameObject;
        eastExplosionObject = EastExplosion.transform.GetChild(0).gameObject;
        westExplosionObject = WestExplosion.transform.GetChild(0).gameObject;


        gameObject.SetActive(false);




        


    }

    private void OnEnable() 
    {
        stageHandler = BattleStageHandler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.GetComponent<BStageEntity>())
        {
            BStageEntity victim = other.GetComponent<BStageEntity>();
            StartCoroutine(ActivateMine());
            if(victim.isBeingShoved)
            {
                TriggerSecondaryExplosions();
                FMODUnity.RuntimeManager.PlayOneShotAttached(BigExplosionSFX, this.gameObject);

            }else
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached(NormalExplosionSFX, this.gameObject);
            }

  
        }    
    }



    IEnumerator ActivateMine()
    {

        MineObjectAnimator.Play("ImpactEffectGroundSplash");
        primaryExplosionObject.SetActive(true);

        yield return new WaitForSecondsRealtime(0.25f);
        gameObject.SetActive(false);
        
    }

    void TriggerSecondaryExplosions()
    {
        if(CheckValidTile(NorthExplosion))
        {
            northExplosionObject.SetActive(true);
        }
        if(CheckValidTile(SouthExplosion))
        {
            southExplosionObject.SetActive(true);
        }
        if(CheckValidTile(EastExplosion))
        {
            eastExplosionObject.SetActive(true);
        }        
        if(CheckValidTile(WestExplosion))
        {
            westExplosionObject.SetActive(true);
        }
    }

    bool CheckValidTile(GameObject explosionObject)
    {
        Vector3Int explosionObjectPosition = new Vector3Int((int)(Math.Round((explosionObject.transform.position.x/1.6f), MidpointRounding.AwayFromZero)),
                            (int)explosionObject.transform.position.y, 0);
        print("tile position: " + explosionObjectPosition);                            

        if(stageHandler.stageTilemap.GetTile
        (explosionObjectPosition) == null)
        {
            return false;
        }
        if(stageHandler.getEntityAtCell(explosionObjectPosition.x, explosionObjectPosition.y) != null &&
        stageHandler.getEntityAtCell(explosionObjectPosition.x, explosionObjectPosition.y).isObstacle)
        {
            return false;
        }


        return true;

    }



}
