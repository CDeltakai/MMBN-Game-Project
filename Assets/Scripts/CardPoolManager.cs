using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPoolManager : MonoBehaviour
{
    //Maintain a reference to the player object in the scene at all times so that initialized effect prefabs or object summons can be given
    //a reference to the player quickly.
    [SerializeField] PlayerMovement player;

    [SerializeField] DeckSO currentActiveDeck;
    //GameObject which all pooled objects will be instantiated under
    [SerializeField] GameObject CardPoolParent;


    //A reference list to all CardObjectReferences that are defined during the pooling process
    //Primarily used by the card selection system in order to populate the menu with selectable cards
    [field:SerializeField] public List<CardObjectReference> CardObjectReferences {get; private set;}

    //A reference list of all currently pooled objects for debugging purposes
    [SerializeField] List<GameObject> PooledObjects = new List<GameObject>();

    private void Awake()
    {
        PoolObjectsFromDeck(currentActiveDeck); 
    }



    //Primary method for pooling all effect prefabs/object summons from a given deck
    private void PoolObjectsFromDeck(DeckSO deck)
    {

        foreach(DeckElement deckElement in deck.CardList)
        {
            //If the deck element has less than or equal to 0 card count, issue a warning and continue to the next deck element.
            if(deckElement.cardCount <= 0)
            {
                Debug.LogWarning("Deck element of card: " + deckElement.card.ChipName + "in Deck: " + deck.DeckName
                + "has a card count of less then or equal to 0, which should not happen. Remove the offending deck"
                + "element or increase its count past 0 to fix this warning.");
                continue;
            }
            //Make sure that the card has an EffectPrefab defined in its cardSO, otherwise continue to the next deck element
            if(deckElement.card.GetEffectPrefab() == null)
            {
                Debug.LogWarning("Card: " + deckElement.card.ChipName + " does not have an EffectPrefab defined in its CardSO, card will not work.");
                continue;
            }

            //Begin instantiating effect prefabs/object summons of the card and repeat card count many times.
            for(int i = 0; i < deckElement.cardCount; i++)
            {
                CardEffect effectPrefab = Instantiate(deckElement.card.GetEffectPrefab(), CardPoolParent.transform).GetComponent<CardEffect>();
                effectPrefab.player = player;
                effectPrefab.quantifiableEffects = deckElement.card.QuantifiableEffects;

                //Check if the card has the ObjectSummonsArePooled condition ticked and it has objects within its ObjectSummonList,
                //then pool objects within the list if conditions are met.
                if(deckElement.card.ObjectSummonsArePooled && deckElement.card.ObjectSummonList.Count > 0)
                {
                    foreach(GameObject objectSummon in deckElement.card.ObjectSummonList)
                    {
                        //Add references to the object summons to the effectPrefab so that it may access the objects.
                        effectPrefab.ObjectSummonList.Add(Instantiate(objectSummon, CardPoolParent.transform));
                        PooledObjects.Add(objectSummon);
                    }
                }
            
                PooledObjects.Add(effectPrefab.gameObject);

                CardObjectReference cardObject;


                    //Create a new CardObjectReference that references the instantiated EffectPrefab and its ObjectSummons
                    cardObject = new CardObjectReference
                    {
                        chipSO = deckElement.card,
                        effectPrefab = effectPrefab.gameObject,
                        ObjectSummonList = effectPrefab.ObjectSummonList
                        
                    };

                


                CardObjectReferences.Add(cardObject);

            }


        }

        //All pooled effect prefabs and object summons start disabled by default, using a card will enable their corresponding effect prefab. 
        foreach(GameObject gameObject in PooledObjects)
        {
            gameObject.SetActive(false);
        }

    }



}
