using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BattleStageHandler : MonoBehaviour
{

    public static BattleStageHandler instance;





    [SerializeField] public Tilemap stageTilemap;
    [SerializeField] Tile defaultTile;
    [SerializeField] public Tile PlayerTile;
    [SerializeField] public Tile NPCTile;




    PlayerMovement player;

    BattleStageNPCInventory NPCInventory;
    TilemapCollider2D tilemapCollider2D;
    Grid grid;

    BoundsInt bounds;
    BoundsInt playerBounds;
    BoundsInt NPCBounds;

    Vector3Int playerPosition;
    Vector3Int worldCellBounds;

    TileBase[] tileArray;

    public Dictionary<Vector3, StageTile> stageTiles;
 

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        GetStageTiles();


    }


    void Start()
    {
        tilemapCollider2D = GetComponent<TilemapCollider2D>();
        NPCInventory = GetComponent<BattleStageNPCInventory>();
        grid = GetComponentInParent<Grid>();
        player = FindObjectOfType<PlayerMovement>();
        
        bounds = stageTilemap.cellBounds;
        

        Debug.Log("Bounds coordinates: " +bounds.ToString());
        //battleStageTilemap.SetTile(new Vector3Int(0, 0, 0), PlayerTile);

        //Vector3 testPosition = battleStageTilemap.GetCellCenterWorld(new Vector3Int(4,1,0));

        //Instantiate(NPCInventory.getNPCObject(0), testPosition, Quaternion.identity);

        
        
    }


    private void GetStageTiles()
    {
        stageTiles = new Dictionary<Vector3, StageTile>();


        foreach (Vector3Int pos in stageTilemap.cellBounds.allPositionsWithin)
        {

            var localPos = new Vector3Int(pos.x, pos.y, pos.z);

            if(!stageTilemap.HasTile(localPos)) {continue;}

            var tile = new StageTile
            {
                localCoords = localPos,
                worldPosition = stageTilemap.CellToWorld(localPos),
                tileBase = stageTilemap.GetTile(localPos),
                TilemapMember = stageTilemap,
                tileName = localPos.x + ", " + localPos.y
            };

            stageTiles.Add(tile.worldPosition, tile);

        }

    }

    
    public void addTile(StageTile tile, Vector3 position)
    {
        stageTiles.Add(position, tile);
    }


    public void setCellOccupied(int x, int y, bool condition)
    {
        Vector3Int cell = new Vector3Int(x, y, 0);

        stageTiles[stageTilemap.CellToWorld(cell)].isOccupied = condition;


    }

    public void setOccupied(StageTile tile, bool condition)
    {
        tile.isOccupied = condition;
    }


    // Update is called once per frame
    void Update()
    {
        


    }
}
