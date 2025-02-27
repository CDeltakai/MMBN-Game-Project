using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class TileEventManager : MonoBehaviour
{

    BattleStageHandler stageHandler;
    Dictionary<BStageEntity, Coroutine> EntityCoroutineList = new Dictionary<BStageEntity, Coroutine>(); 



    

    IEnumerator testCO()
    {
        yield return null;
        
    }

    void Awake()
    {
        stageHandler = GetComponent<BattleStageHandler>();
    }

    void Start()
    {
        foreach(BStageEntity entity in stageHandler.EntityList)
        {
            entity.moveOntoTile += MoveOntoTileEffect;
            entity.moveOffTile += MoveOffTileEffect;

        }
    }

    public void UnsubscribeEntity(BStageEntity entity)
    {
        entity.moveOntoTile -= MoveOntoTileEffect;
        entity.moveOffTile -= MoveOffTileEffect;
    }



    public void MoveOntoTileEffect(int x, int y, BStageEntity entity)
    {

        Vector3Int cell = new Vector3Int(x, y, 0);

        CustomTile tile = stageHandler.getCustTile(cell);

        switch(tile.GetTileEnum())
        {

            case ETiles.Poison_Tile:
                entity.DamageEntity(1, 0.5f, 2f);

                break;



        }


    }

    public void MoveOffTileEffect(int x, int y, BStageEntity entity)
    {

        Vector3Int cell = new Vector3Int(x, y, 0);

        CustomTile tile = stageHandler.getCustTile(cell);

        switch(tile.GetTileEnum())
        {
            case ETiles.Cracked_Tile:
                CrackTile(cell, tile.tileTeam);
                break;
            case ETiles.Poison_Tile:

                break;



        }


    }



    void CrackTile(Vector3Int cell, ETileTeam tileTeam)
    {

        if(stageHandler.getCustTile(cell).GetTileEnum() == ETiles.Cracked_Tile)
        {
            stageHandler.SetCustomTile(cell, ETiles.Cracked_Empty_Tile, tileTeam);
            
        }else
        {
            stageHandler.SetCustomTile(cell, ETiles.Cracked_Tile, tileTeam);

        }

    }

    void Update()
    {

    }






}
