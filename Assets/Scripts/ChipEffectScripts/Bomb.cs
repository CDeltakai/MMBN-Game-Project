using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour, IChip
{

    BattleStageHandler stageHandler;
    PlayerMovement player;
    public Tilemap stageTilemap;
    public CustomTile tile;
    public int AdditionalDamage{get; set;} = 0;
    public int BaseDamage {get;} = 60;

    public EChipTypes ChipType => EChipTypes.Attack;

    public EChipElements chipElement => EChipElements.Normal;

    public EStatusEffects chipStatusEffect {get;set;} = EStatusEffects.Default;

    void Awake()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        player = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {
        Addressables.InstantiateAsync(AddressableKey, new Vector2(player.worldTransform.position.x, player.worldTransform.position.y), transform.rotation);

        

    }
}
