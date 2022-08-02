using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; 

public class Longsword : MonoBehaviour, IChip
{
    public Transform firePoint;
    BattleStageHandler stageHandler;
    public int BaseDamage {get;set;} = 80;

    public int AdditionalDamage{get; set;} = 0;

    public EChipTypes ChipType => EChipTypes.Active;
    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;
    public EChipElements chipElement => EChipElements.Blade;

    PlayerMovement player;
    public Vector3 initPosition;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        stageHandler = FindObjectOfType<BattleStageHandler>();
        initPosition.Set(player.transform.localPosition.x + 1.6f, player.transform.localPosition.y, 0);
        firePoint.localPosition = initPosition;

    }


    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default)
    {

        Addressables.InstantiateAsync("VFX_Longsword_pos", firePoint);
    }

    
}
