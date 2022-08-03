using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaGrab : MonoBehaviour, IChip
{

    public Tilemap tiles;
    public Tile tile;
    public int AdditionalDamage{get; set;} = 0;
    public int BaseDamage {get;} = 10;

    public EChipTypes ChipType => EChipTypes.Special;

    public EChipElements chipElement => EChipElements.Normal;

    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;

    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {
        throw new System.NotImplementedException();
    }


}
