using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaGrab : MonoBehaviour, IChip
{

    BattleStageHandler stageHandler;

    public Tilemap stageTilemap;
    public CustomTile tile;
    public int AdditionalDamage{get; set;} = 0;
    public int BaseDamage {get;} = 10;

    public EChipTypes ChipType => EChipTypes.Special;

    public EChipElements chipElement => EChipElements.Normal;

    public EStatusEffects statusEffect {get;set;} = EStatusEffects.Default;

    private void Awake() 
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        stageTilemap = stageHandler.stageTilemap;
        //tile = stageHandler.PlayerTiles.Find(tile => tile.GetTileEnum() == ETiles.Player_DefaultTile);
    }


//NOTE: This AreaGrab will ONLY work with Variable Tiles
    public void Effect(int AddDamage = 0, EStatusEffects statusEffect = EStatusEffects.Default, string AddressableKey = null)
    {
        stageHandler.CalculatePlayerBounds();

        print("Attempted area grab");
        foreach(Vector3Int pos in stageHandler.playerBoundsList)
        {
            Vector3Int localPos = new Vector3Int(pos.x + 1, pos.y, 0);
            tile = stageTilemap.GetTile<CustomTile>(localPos);
            tile.switchToTileTeam(ETileTeam.Player);


            stageTilemap.SetTile(new Vector3Int(pos.x + 1, pos.y, 0) , tile);
        }

    }


}
