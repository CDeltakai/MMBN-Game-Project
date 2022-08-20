using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, IChip
{

    BattleStageHandler stageHandler;

    public Tilemap stageTilemap;
    public CustomTile tile;
    public int AdditionalDamage{get; set;} = 0;
    public int BaseDamage {get;} = 60;

    public EChipTypes ChipType => EChipTypes.Active;

    public EChipElements chipElement => EChipElements.Normal;

    public EStatusEffects chipStatusEffect {get;set;} = EStatusEffects.Default;

    void Awake()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {



    }
}
