using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshotEffect : CardEffect
{

    [SerializeField] QuantifiableEffect ShoveDistance;
    [SerializeField] GameObject projectilePrefab;
    private AirshotProjectile airshotProjectile;

    protected override void Awake()
    {
        base.Awake();
        ShoveDistance = quantifiableEffects[0];


    }


    public override void ActivateCardEffect()
    {
        airshotProjectile = Instantiate(projectilePrefab, new Vector2(player.worldTransform.position.x + 1.6f,
        player.worldTransform.position.y), transform.rotation).GetComponent<AirshotProjectile>();

        airshotProjectile.attackPayload = CalculateFinalPayload();
        airshotProjectile.player = player;
        airshotProjectile.ShoveDistance = ShoveDistance.IntegerQuantity;

        StartCoroutine(DisableEffectPrefab());
    }

    protected override IEnumerator DisableEffectPrefab()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        gameObject.SetActive(false);
    }


    
}
