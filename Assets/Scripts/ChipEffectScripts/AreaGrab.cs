using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AreaGrab : ChipEffectBlueprint
{

    BattleStageHandler stageHandler;

    private Tilemap stageTilemap;
    private CustomTile tile;
    [SerializeField] AreaGrabVFXController vfxController;
    public float column;

    private void Awake() 
    {
        //stageHandler = BattleStageHandler.Instance;
        stageHandler = FindObjectOfType<BattleStageHandler>();
        stageTilemap = stageHandler.stageTilemap;

    }


//NOTE: This AreaGrab will ONLY work with Variable Tiles


    public override void Effect()
    {

        StartCoroutine(TimedEffect());


    }

    IEnumerator TimedEffect()
    {
        stageHandler.CalculatePlayerBounds();
        column = (stageHandler.playerBoundsList[0].x + 1) * 1.6f;

        vfxController.InitialPosition = new Vector3(column, 10, 0);

        vfxController.gameObject.SetActive(true);

        vfxController.TriggerVFX();

        yield return new WaitForSeconds(0.5f);
        print("Attempted area grab");
        foreach(Vector3Int pos in stageHandler.playerBoundsList)
        {

            Vector3Int localPos = new Vector3Int(pos.x + 1, pos.y, 0);
            StageTile stageTileToCheck = stageHandler.stageTiles
            [stageHandler.stageTilemap.CellToWorld(localPos)];

            if(localPos.x >= 6 || stageTileToCheck.entity != null || stageTileToCheck.entityClaimant != null)
            {
                continue;
            }
            tile = stageTilemap.GetTile<CustomTile>(localPos);
            


            stageTilemap.SetTile(new Vector3Int(pos.x + 1, pos.y, 0) , tile.getCustomPlayerTile());
            
        }
        stageHandler.CalculatePlayerBounds();

        vfxController.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }




}
