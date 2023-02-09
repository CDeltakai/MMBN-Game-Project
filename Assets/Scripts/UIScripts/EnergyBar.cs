using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using TMPro;
public class EnergyBar : MonoBehaviour
{





    [SerializeField] PlayerAttributeSO PlayerAttributes;
    [SerializeField] TextMeshProUGUI EnergyText;
    PlayerMovement player;

    Image BarImage;

    public int MaxEnergy;
    public int currentEnergy;

    public bool animatingBar;
    float animationtime = 1f;
    float elapsedTime = 0;

    float startFillAmount;
    [SerializeField] int cost = 1;
    [SerializeField] float end = 0.9f;
    
    private void Awake()
    {
        BarImage = GetComponent<Image>();
        BarImage.fillAmount = 1f;

    }


    void Start()
    {
        if(PlayerMovement.Instance != null)
        {
            player = PlayerMovement.Instance;
        }else
        {
            player = FindObjectOfType<PlayerMovement>();
        }

        MaxEnergy = PlayerAttributes.AdjustOrGetMaxEnergy();
        currentEnergy = player.currentEnergy;
   
    }

    // Update is called once per frame
    void Update()
    {

        if(animatingBar)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float percentageComplete = elapsedTime/animationtime;

            BarImage.fillAmount = Mathf.Lerp(cost, end, percentageComplete);
            

            if(elapsedTime > 1)
            {
                elapsedTime = 0f;
                animationtime = 1f;
                animatingBar = false;
            }

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

    public void AnimateBarUpdate(int amount)
    {
        

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
