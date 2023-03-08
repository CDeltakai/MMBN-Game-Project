using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStageNPCInventory : MonoBehaviour
{
    [SerializeField] List<UnityEngine.GameObject> EnemyList;


    public UnityEngine.GameObject getNPCObject(int element)
    {
        return EnemyList[element];


    }



}
