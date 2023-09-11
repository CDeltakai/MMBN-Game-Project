using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class CardObjectReference
{
    public ChipSO chipSO;
    public GameObject effectPrefab;
    public GameObject ObjectSummon;
    public List<GameObject> ObjectSummonList;
    [SerializeReference] List<CardObjectReference> attachedPassiveCards = new List<CardObjectReference>();


    [SerializeReference] public CardSlot cardSlot;


    public void AttachPassiveCard(CardObjectReference passiveCard)
    {
        //Make sure that both this card and the incoming card are of the correct type in order to attach onto this card.
        if(chipSO.ChipType != EChipTypes.Active)
        {
            Debug.LogWarning(chipSO.ChipName + " is not an active card, passive cards cannot be attached to it.");
            return;
        }
        if(passiveCard.chipSO.ChipType != EChipTypes.Passive)
        {
            Debug.LogWarning("Cannot attach " + passiveCard.chipSO.ChipName + " to " + chipSO.ChipName + ", " + passiveCard.chipSO.ChipName +
            "is not a passive card.");
            return;
        }

        attachedPassiveCards.Add(passiveCard);

    }

    public void ClearPassiveCards()
    {
        attachedPassiveCards.Clear();
    }


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

    //[SerializeReference] public List<ActiveCardObjReference> attachedPassiveCards = new List<ActiveCardObjReference>();





}




[System.Serializable]
public class PassiveCardObjReference : CardObjectReference
{




}