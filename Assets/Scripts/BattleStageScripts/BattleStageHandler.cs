using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using UnityEditor.UIElements;
using Pathfinding.Util;

public class BattleStageHandler : MonoBehaviour
{

    public static BattleStageHandler instance;





    [SerializeField] public Tilemap stageTilemap;
    [SerializeField] Tile defaultTile;
    [SerializeField] public Tile PlayerTile;
    [SerializeField] public Tile NPCTile;
    [SerializeField] public List<CustomTile> PlayerTiles;
    [SerializeField] public List<CustomTile> NPCTiles;



    PlayerMovement player;

    BattleStageNPCInventory NPCInventory;
    TilemapCollider2D tilemapCollider2D;
    public Grid grid;

    BoundsInt bounds;

    Vector3Int playerPosition;
    Vector3Int worldCellBounds;

    CustomTile[] tileLoad;
    public Dictionary<Vector3, StageTile> stageTiles;

    //int value is the row
    public Dictionary<Vector3Int, int> playerBoundsDict = new Dictionary<Vector3Int, int>();
    public List<Vector3Int> playerBoundsList = new List<Vector3Int>();

    

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
        PlayerTiles.Clear();
        NPCTiles.Clear();
        LoadTiles();
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
        getCustTile(new Vector3Int(0, 0, 0));
        CalculatePlayerBounds();

        
        //battleStageTilemap.SetTile(new Vector3Int(0, 0, 0), PlayerTile);

        //Vector3 testPosition = battleStageTilemap.GetCellCenterWorld(new Vector3Int(4,1,0));

        //Instantiate(NPCInventory.getNPCObject(0), testPosition, Quaternion.identity);

        
        
    }

    public void CalculatePlayerBounds()
    {
        playerBoundsDict.Clear();
        playerBoundsList.Clear();
        print("Attempting to calculate player bounds");
        Vector3Int coordToCheck = new Vector3Int(1,0,0);

        for(int row = 0; row < 4; row++)
        {
            coordToCheck.Set(1, row, 0);

            while(   stageTilemap.GetTile<CustomTile>(coordToCheck).GetTileTeam() == ETileTeam.Player )
            {
                coordToCheck.Set(coordToCheck.x + 2, row, 0);

                if(stageTilemap.GetTile<CustomTile>(coordToCheck).GetTileTeam() == ETileTeam.Enemy)
                {
                    coordToCheck.Set(coordToCheck.x - 1, row, 0);
                    if(stageTilemap.GetTile<CustomTile>(coordToCheck).GetTileTeam() == ETileTeam.Player)
                    {
                        playerBoundsDict.Add(new Vector3Int(coordToCheck.x, row, 0), row);
                        playerBoundsList.Add(new Vector3Int(coordToCheck.x, row, 0));
                        //print("Added player bounds at: " + coordToCheck.ToString() + "at row: " +row);
                        break;
                    }else
                    {
                        playerBoundsDict.Add(new Vector3Int(coordToCheck.x - 1, row, 0), row);
                        playerBoundsList.Add(new Vector3Int(coordToCheck.x - 1, row, 0));

                        //Vector3Int stringVector = new Vector3Int(coordToCheck.x - 1, row, 0);
                        //print("Added player bounds at: " + stringVector.ToString() + "at row: " +row);

                        break;
                    }

                }

            }

            
        }

    }


    //Should load CustomTiles from the Resources/Tiles folder and place the tiles into
    //corresponding NPCTiles/PlayerTiles list
    public void LoadTiles()
    {
        tileLoad = Resources.LoadAll<CustomTile>("Tiles");

        foreach(var tile in tileLoad)
        {
            if(tile.GetTileTeam() == ETileTeam.Player)
            {
                PlayerTiles.Add(tile);
            }else
            {
                NPCTiles.Add(tile);
            }

        }


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

    public CustomTile getCustTile(Vector3Int position)
    {
        var custTile = stageTilemap.GetTile(position);
        if (custTile is CustomTile)
        {
            var selectedTile = (CustomTile) custTile;
            //print("Tile at " + position.ToString() + "Is a custom tile, name:" + selectedTile.tileSO.GetName());
            return selectedTile;

        }else
        {
            print("Tile at "+ position.ToString() + "is not a CustomTile, retuned null");
        }

        return null;


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
