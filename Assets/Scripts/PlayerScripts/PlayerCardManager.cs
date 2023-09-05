using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCardManager : MonoBehaviour
{
    //Event that is triggered whenever a card is removed from the magazine
    public delegate void OnRemoveCard();
    public event OnRemoveCard RemoveCardEvent;

    //Event that is triggered whenever the magazine is loaded
    public delegate void OnLoadMagazine();
    public event OnLoadMagazine LoadMagazineEvent;

    PlayerAnimationController animationController;

    //The list of cards the player can use in battle. Is loaded during the Card Selection Menu.
    [field:SerializeField] public List<CardObjectReference> cardMagazine{get; private set;} = new List<CardObjectReference>(); 

    [SerializeField] float defaultCooldown = 0.15f;

    public bool CanUseCards = true;

    private void Awake()
    {
        animationController = GetComponent<PlayerAnimationController>();    
    }

    void Start()
    {
        
    }

    //Clears the cardMagazine and then loads a given cardLoad into the cardMagazine
    public void LoadCardMagazine(List<CardObjectReference> cardLoad)
    {
        cardMagazine.Clear();

        foreach(CardObjectReference card in cardLoad)
        {
            cardMagazine.Add(card);
        }

        //Raise LoadMagazineEvent event if at least one other object is subscribed
        LoadMagazineEvent?.Invoke();

    }

    //Loads a single card into the bottom of the magazine 
    public void LoadOneCard(CardObjectReference card)
    {
        cardMagazine.Add(card);
    }

    //A gatekeeper method that checks what kind of card the next card to be used is and calls the correct method to use said card.
    public void TriggerCard()
    {
        CardObjectReference cardToUse = cardMagazine[0];

        //If cardToUse has an animation defined, use the animationController to play said animation, otherwise call the UseCardOneShot method.
        if(!cardToUse.chipSO.UseAnimationEvent)
        {
            UseCardOneShot();
        }else
        {
            animationController.PlayAnimationClip(cardToUse.chipSO.GetAnimationClip());
        }

    }


    //Uses the first card in the magazine and then immediately removes it from the magazine.
    //Only use this method for cards that do not use a player animation to trigger its effect 
    public void UseCardOneShot()
    {
        if(!CanUseCards)
        {return;}

        CardEffect cardToUse = cardMagazine[0].effectPrefab.GetComponent<CardEffect>();

        cardToUse.gameObject.SetActive(true);
        cardToUse.ActivateCardEffect();

        //Trigger card use cooldown, preventing the next card from being used until cooldown is complete
        CanUseCards = false;
        StartCoroutine(Cooldown(defaultCooldown));

        StartCoroutine(RemoveCardFromMagazine(cardMagazine[0]));        

    }

    //Method that is called on an animation event of a player animation which is used for a card effect
    public void UseCardAnimationEvent()
    {
        //Checks if the first card actually has a player animation defined, otherwise will return. This is meant to allow player animations
        //that are used for card effects to be able to be used for other things without accidentally triggering this method.
        if(cardMagazine[0].chipSO.GetAnimationClip() == null)
        {return;}

        CardEffect cardToUse = cardMagazine[0].effectPrefab.GetComponent<CardEffect>();

        cardToUse.gameObject.SetActive(true);
        cardToUse.ActivateCardEffect();

        //Trigger card use cooldown, uses the animation length as the duration plus the default cooldown
        CanUseCards = false;
        StartCoroutine(Cooldown(cardToUse.chipSO.GetAnimationClip().length));

        StartCoroutine(RemoveCardFromMagazine(cardMagazine[0]));    

    }



    IEnumerator Cooldown(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        CanUseCards = true;   
    }

    //Removes the given card from the magazine, can be given an optional duration to remove the card only after said duration
    IEnumerator RemoveCardFromMagazine(CardObjectReference card, float duration = 0)
    {
        yield return new WaitForSecondsRealtime(duration);
        cardMagazine.Remove(card);

        //Invoke event if it is not null
        RemoveCardEvent?.Invoke();
    }





    public bool MagazineEmpty()
    {
        if(cardMagazine.Count == 0)
        {
            return true;
        }
        return false;
    }


}
