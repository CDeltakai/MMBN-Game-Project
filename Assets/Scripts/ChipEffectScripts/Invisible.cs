using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : ChipEffectBlueprint
{

    Color cloakedColor;




   void Start()
    {
        player.playerHurtEvent += cancelInvisible;
        
    }



    public override void Effect()
    {

        StartCoroutine(CastInvisible());

    }


    IEnumerator CastInvisible()
    {
        cloakedColor = player.defaultColor;
        cloakedColor.a = 0.6f;

        player.SetUntargetable(true);
        player.spriteRenderer.color = cloakedColor;
        print("Attempted invisible");
        yield return new WaitForSeconds(10);

        player.SetUntargetable(false);
        player.spriteRenderer.color = player.defaultColor;
        this.gameObject.SetActive(false);




    }

    void cancelInvisible()
    {
        StopCoroutine(CastInvisible());
        player.spriteRenderer.color = player.defaultColor;
        player.SetUntargetable(false);
        this.gameObject.SetActive(false);


    }

 

    void Update()
    {
        
    }
}
