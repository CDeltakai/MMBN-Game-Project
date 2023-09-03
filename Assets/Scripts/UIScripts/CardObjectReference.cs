using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardObjectReference
{
    public ChipSO chipSO;
    public GameObject effectPrefab;
    public GameObject ObjectSummon;
    public List<GameObject> ObjectSummonList;
    public CardSlot cardSlot;

    public void ClearReferences()
    {
        chipSO = null;
        effectPrefab = null;
        ObjectSummon = null;
        ObjectSummonList = null;
    }



}
