using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using FMODUnity;

public class Generic_Sword : MonoBehaviour, IChip
{
    BattleStageHandler stageHandler;
    public int BaseDamage {get;set;} = 80;
    public int AdditionalDamage{get; set;} = 0;

    public EChipTypes ChipType => EChipTypes.Attack;
    public EStatusEffects chipStatusEffect {get;set;} = EStatusEffects.Default;
    public AttackElement chipElement => AttackElement.Blade;
    //[SerializeField] GameObject vfx; 

    PlayerMovement player;
    public Vector2 initPosition;


    void Awake() {
        player = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        
        stageHandler = FindObjectOfType<BattleStageHandler>();        
    }


    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {
        Addressables.InstantiateAsync(AddressableKey, new Vector2(player.worldTransform.position.x + 1.6f, player.worldTransform.position.y), transform.rotation);
        //Instantiate(vfx, initPosition, transform.rotation);
    }
}
