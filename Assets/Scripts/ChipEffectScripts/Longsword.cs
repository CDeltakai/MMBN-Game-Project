using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets; 

public class Longsword : MonoBehaviour, IChip
{
    
    public AssetReference reference;
    BattleStageHandler stageHandler;
    public int BaseDamage {get;set;} = 80;

    public int AdditionalDamage{get; set;} = 0;

    public EChipTypes ChipType => EChipTypes.Active;
    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;
    public EChipElements chipElement => EChipElements.Blade;
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


    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default)
    {
        Addressables.InstantiateAsync("VFX_Longsword_pos", new Vector2(player.parentTransform.position.x + 1.6f, player.parentTransform.position.y), transform.rotation);
        //Instantiate(vfx, initPosition, transform.rotation);
    }


}
