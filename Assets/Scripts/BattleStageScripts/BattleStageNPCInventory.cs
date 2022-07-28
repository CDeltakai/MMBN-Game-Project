using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStageNPCInventory : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyList;


    public GameObject getNPCObject(int element)
    {
        return EnemyList[element];


    }



}
