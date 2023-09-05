using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardObjectReference
{
    public ChipSO chipSO;
    public GameObject effectPrefab;
    public List<CardObjectReference> attachedPassiveCards = new List<CardObjectReference>();
    public GameObject ObjectSummon;
    public List<GameObject> ObjectSummonList;
    public CardSlot cardSlot;

    public void ClearReferences()
    {
        chipSO = null;
        effectPrefab = null;
        attachedPassiveCards.Clear();
        ObjectSummon = null;
        ObjectSummonList.Clear();
    }

    public void ClearPassiveCards()
    {
        attachedPassiveCards.Clear();
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
