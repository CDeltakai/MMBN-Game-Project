using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using TMPro;
using System;

public class EnergyBar : MonoBehaviour
{





    [SerializeField] PlayerAttributeSO PlayerAttributes;
    [SerializeField] TextMeshProUGUI EnergyText;
    [SerializeField] PlayerMovement player; 

    Image BarImage;

    public int MaxEnergy;
    public int currentEnergy;

    public bool animatingBar;
    bool triggeredBar = false;


    public float animationtime = 0.2f;
    float elapsedTime = 0;

    float startFillAmount;
    [SerializeField] float start = 1;
    [SerializeField] float end = 1;
    
    private void Awake()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
            Debug.LogWarning("Energy bar does not have the player script set manually,"
             + "used FindObjectOfType to find main player script. Please manually set the player script for better efficiency.");
        }
        
        BarImage = GetComponent<Image>();
        BarImage.fillAmount = 1f;
        player.usedChipEvent += TriggerBarUpdate;

    }


    void Start()
    {

        MaxEnergy = PlayerAttributes.AdjustOrGetMaxEnergy();
        currentEnergy = player.currentEnergy;
   
    }

    // Update is called once per frame
    void Update()
    {

        AnimateBarUpdate();
        

    }
    public void AnimateBarUpdate()
    {

        if(animatingBar)
        {

            elapsedTime += Time.unscaledDeltaTime;
            float percentageComplete = elapsedTime/animationtime;

            BarImage.fillAmount = Mathf.Lerp(start, end, percentageComplete);
            

            if(elapsedTime > animationtime)
            {
                elapsedTime = 0f;
                animatingBar = false;

            }

        }        

    }

    public void TriggerBarUpdate()
    {

    }

    public void InitializeBar(int energyCost)
    {
        if(energyCost == 0)
        {
            return;
        }

        start = BarImage.fillAmount;

        if(energyCost < 0)
        {
            if(Math.Abs(energyCost)  > player.currentEnergy)
            {
                end = 0f;
                animatingBar = true;
                EnergyText.text = (player.currentEnergy.ToString() + "/" + player.MaxEnergy.ToString());

                return;
            }

            end  = start + ((float)energyCost/(float)player.MaxEnergy);
            animatingBar = true;
            EnergyText.text = (player.currentEnergy.ToString() + "/" + player.MaxEnergy.ToString());

        }else
        {
            if(player.currentEnergy + energyCost > player.MaxEnergy)
            {
                end = 1;
                animatingBar = true;
                EnergyText.text = (player.currentEnergy.ToString() + "/" + player.MaxEnergy.ToString());

                return;
            }

            end  = start + ((float)energyCost/(float)player.MaxEnergy);
            animatingBar = true;
            EnergyText.text = (player.currentEnergy.ToString() + "/" + player.MaxEnergy.ToString());
                        
        }

        

    }



    public void AdjustEnergy(int amount)
    {
        if(amount == 0)
        {
            return;
        }

        float fillAdjustAmount = (float)amount/(float)MaxEnergy;
        currentEnergy = player.currentEnergy;
        if(currentEnergy < 0)
        {
            currentEnergy = 0;
        }else if
        (currentEnergy > player.MaxEnergy)
        {
            currentEnergy = player.MaxEnergy;
        }

        EnergyText.text = (currentEnergy.ToString() + "/" + player.MaxEnergy.ToString());
        
        //BarImage.DOFillAmount(BarImage.fillAmount += fillAdjustAmount, 0.2f);

        StartCoroutine(AnimateBarFillAmount(amount, 0.2f));

    }



    IEnumerator AnimateBarFillAmount(int energyCost, float duration)
    {
        float fillAdjustAmount = (float)energyCost/(float)MaxEnergy;
        float fillTickRate = duration/energyCost;
        float finalFillAmount = BarImage.fillAmount += fillAdjustAmount;

        if(energyCost > 0)
        {
            while(BarImage.fillAmount > finalFillAmount)
            {
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
                BarImage.fillAmount -= 0.05f;

            }

        }else
        {
            while(BarImage.fillAmount < finalFillAmount)
            {
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

                BarImage.fillAmount += 0.05f;
            }            
        }



    }

}
