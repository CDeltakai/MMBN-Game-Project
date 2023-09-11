using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckElement
{
    public string CardName;
    public ChipSO card;
    public int cardCount;

}

[CreateAssetMenu(fileName = "Deck Data", menuName = "New Deck", order = 0)]
public class DeckSO : ScriptableObject
{
    [field:SerializeField] public string DeckName{get; private set;}


    [field:SerializeField] public List<DeckElement> CardList {get; private set;}


}
