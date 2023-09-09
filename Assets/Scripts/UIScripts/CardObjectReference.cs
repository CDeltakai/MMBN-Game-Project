using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        ObjectSummonList.Clear();
    }


    public bool IsEmpty()
    {
        if(chipSO == null)
        {
            return true;
        }
        return false;
    }

}


[System.Serializable]
public class ActiveCardObjReference : CardObjectReference
{

    [SerializeReference] public List<ActiveCardObjReference> attachedPassiveCards = new List<ActiveCardObjReference>();





}




[System.Serializable]
public class PassiveCardObjReference : CardObjectReference
{




}