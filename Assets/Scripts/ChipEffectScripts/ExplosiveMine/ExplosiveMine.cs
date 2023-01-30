using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : ObjectSummonAttributes
{
    [SerializeField] BattleStageHandler stageHandler;
    [SerializeField] GameObject MineObject;
    [SerializeField] GameObject PrimaryExplosion;

    [SerializeField] GameObject NorthExplosion;
    [SerializeField] GameObject SouthExplosion;
    [SerializeField] GameObject EastExplosion;
    [SerializeField] GameObject WestExplosion;

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

        gameObject.SetActive(false);




        


    }

    private void OnEnable() 
    {
        stageHandler = BattleStageHandler.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            StartCoroutine(ActivateMine());
        }    
    }



    IEnumerator ActivateMine()
    {

        MineObjectAnimator.Play("ImpactEffectGroundSplash");
        PrimaryExplosion.transform.GetChild(0).gameObject.SetActive(true);
        if(CheckValidTile(NorthExplosion))
        {
            NorthExplosion.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(CheckValidTile(SouthExplosion))
        {
            SouthExplosion.transform.GetChild(0).gameObject.SetActive(true);
        }
        if(CheckValidTile(EastExplosion))
        {
            EastExplosion.transform.GetChild(0).gameObject.SetActive(true);
        }        
        if(CheckValidTile(WestExplosion))
        {
            WestExplosion.transform.GetChild(0).gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(0.25f);
        gameObject.SetActive(false);
        
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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
