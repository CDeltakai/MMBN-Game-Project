using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : MonoBehaviour, IChip
{

    PlayerMovement player;
    ChipEffects chipEffects;

    public int BaseDamage {get;set;} = 60;
    public int AdditionalDamage{get; set;} = 0;
    public EChipTypes ChipType => EChipTypes.Active;
    public EChipElements chipElement => EChipElements.Normal;

    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;


    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {
        AdditionalDamage += AddDamage;

        chipEffects = GetComponent<ChipEffects>();
        player = GetComponent<PlayerMovement>();
        GameObject ParryCollider = chipEffects.ParryCollider;
        Instantiate(ParryCollider, new Vector2 (player.transform.localPosition.x, player.transform.localPosition.y), player.transform.rotation);

    }
}
