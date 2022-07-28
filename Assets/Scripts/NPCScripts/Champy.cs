using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Champy : MonoBehaviour, IBattleStageEntity
{
    public string Name => "Champy";

    public int ID => 1;
    BattleStageHandler stageHandler;
    Animator animator;


    public bool stunnable => false;

    public bool vulnerable { get; set; } = false;
    public Transform worldTransform {get; set;}
    [SerializeField]public float AttackMultiplier {get;set;} = 1;
    [SerializeField]public float DefenseMultiplier {get;set;} = 1;

    public Vector3Int currentCellPos;

    [SerializeField] public int currentHP = 60;
    [SerializeField] TextMeshProUGUI healthText;


    void Awake() {
        worldTransform = transform.parent.gameObject.GetComponent<Transform>();
        currentCellPos.x = (int)(worldTransform.localPosition.x/1.6);
        currentCellPos.y = (int)(worldTransform.localPosition.y);
        
    }

    void Start()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        healthText.text = currentHP.ToString();
        animator = GetComponent<Animator>();
        stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = true;


    }

    public Transform getWorldPosition()
    {
        return worldTransform;
    }


    void Update()
    {
        worldTransform = transform.parent.gameObject.GetComponent<Transform>();
        //print(worldPosition.localPosition.ToString());

    }

    public Vector3Int getCellPosition()
    {
        currentCellPos.x = (int)(worldTransform.localPosition.x/1.6);
        currentCellPos.y = (int)(worldTransform.localPosition.y);
        return currentCellPos;
    }

    public void hurtEntity(int damage, bool lightAttack, bool hitStun, bool pierceCloaking = false)
    {

        int postMitigationDmg = (int)(damage * DefenseMultiplier);

        if(postMitigationDmg >= currentHP)
        {
            currentHP = 0;
            healthText.text = "0";
            stageHandler.stageTiles[stageHandler.stageTilemap.CellToWorld(currentCellPos)].isOccupied = false;

            Destroy(gameObject);
            return;
        }

        currentHP -= postMitigationDmg;
        healthText.text = currentHP.ToString();
   
    }

    public void setCellPosition(int x, int y)
    {
        currentCellPos.Set(x, y, currentCellPos.z);
        worldTransform.transform.localPosition = stageHandler.stageTilemap.
                                    GetCellCenterWorld(currentCellPos);
    }

    public int getHealth()
    {
        return currentHP;
    }
    public void setHealth(int value)
    {
        currentHP = value;
    }

    public IEnumerator setStatusEffect(EStatusEffects status)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(1.5f);
        animator.speed = 1;

    }

}
