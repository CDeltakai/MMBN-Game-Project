using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class TileEventManager : MonoBehaviour
{

    BattleStageHandler stageHandler;




    void Awake()
    {
        stageHandler = GetComponent<BattleStageHandler>();
    }

    void Start()
    {
        foreach(BStageEntity entity in stageHandler.EntityList)
        {
            entity.moveOntoTile += TileEffect;

        }
    }

    public void UnsubscribeEntity(BStageEntity entity)
    {
        entity.moveOntoTile -= TileEffect;
    }



    public void TileEffect(int x, int y)
    {
        Vector3Int cell = new Vector3Int(x, y, 0);

        CustomTile tile = stageHandler.getCustTile(cell);

        switch(tile.GetTileEnum())
        {
            case ETiles.Cracked_Tile:
                CrackTile(cell);
                break;
            case ETiles.Poison_Tile:

                break;



        }




    }


    void CrackTile(Vector3Int cell)
    {

    }








}
