using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_Indestructible : MonoBehaviour
{

    BattleStageHandler stageHandler;
    [HideInInspector] public Transform parentTransform;
    public Vector3Int currentCellPos;



    void Start()
    {
        stageHandler = FindObjectOfType<BattleStageHandler>();
        parentTransform = transform.parent.gameObject.GetComponent<Transform>();
        currentCellPos.x = (int)(parentTransform.localPosition.x/1.6f);
        currentCellPos.y = (int)parentTransform.localPosition.y;
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, true);



    }


    public void removeObject()
    {
        stageHandler.setCellOccupied(currentCellPos.x, currentCellPos.y, false);
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
