using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CardObjectReference
{
    public ChipSO chipSO;
    public GameObject effectPrefab;
    [SerializeField] CardObjectReference passiveCard1;
    [SerializeField] CardObjectReference passiveCard2;
    [SerializeField] CardObjectReference passiveCard3;
    [SerializeField] CardObjectReference passiveCard4;
    public GameObject ObjectSummon;
    public List<GameObject> ObjectSummonList;
    public CardSlot cardSlot;

    public void ClearReferences()
    {
        chipSO = null;
        effectPrefab = null;
        
        passiveCard1 = null;
        passiveCard2 = null;
        passiveCard3 = null;
        passiveCard4 = null;        


        ObjectSummon = null;
        ObjectSummonList.Clear();
    }

    public void ClearPassiveCards()
    {
        passiveCard1 = null;
        passiveCard2 = null;
        passiveCard3 = null;
        passiveCard4 = null;   

    }

    public bool IsEmpty()
    {
        if(chipSO == null)
        {
            return true;
        }
        return false;
    }

    public void AddCardToPassives(CardObjectReference card)
    {
        if(passiveCard1 == null || passiveCard1.IsEmpty())
        {
            passiveCard1 = card;
        }else
        if(passiveCard2 == null || passiveCard2.IsEmpty())
        {
            passiveCard2 = card;
        }else
        if(passiveCard3 == null || passiveCard3.IsEmpty())
        {
            passiveCard3 = card;
        }else
        if(passiveCard4 == null || passiveCard4.IsEmpty())
        {
            passiveCard4 = card;
        }

    }

    public List<CardObjectReference> GetPassiveCards()
    {
        List<CardObjectReference> passiveCards = new List<CardObjectReference>
        {
            passiveCard1
        };
        return passiveCards;
    }

}
