using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlus10Effect : CardEffect
{
    public override void ActivateCardEffect()
    {
        CardEffect activeCardToBuff = cardObjectReference.GetActiveCardTether().effectPrefab.GetComponent<CardEffect>();
        activeCardToBuff.damageModifier += quantifiableEffects[0].IntegerQuantity;
        cardObjectReference.BreakActiveCardTether();
        StartCoroutine(DisableEffectPrefab());

    }

    protected override IEnumerator DisableEffectPrefab()
    {
        gameObject.SetActive(false);
        yield break;
    }
}
