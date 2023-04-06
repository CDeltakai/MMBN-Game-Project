using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingReticleController : MonoBehaviour
{
    
    [SerializeField] PlayerMovement player;
    ChipLoadManager chipLoadManager;
    [SerializeField] List<GameObject> AimingReticules;


    void Awake()
    {
        foreach(GameObject reticule in AimingReticules)
        {
            reticule.SetActive(false);
        }

        chipLoadManager = player.GetComponent<ChipLoadManager>();
        chipLoadManager.loadChipsEvent += TriggerUpdateReticule;
        player.usedChipEvent += TriggerUpdateReticule;        

    }

    void Start()
    {
        
    }

    public void TriggerUpdateReticule()
    {
        if(chipLoadManager.nextChipRefLoad.Count > 0)
        {
            UpdateReticule(chipLoadManager.nextChipRefLoad[0].chipSORef);
        }else
        {
            foreach(GameObject reticule in AimingReticules)
            {
                reticule.SetActive(false);
            }            
        }
    }

    public void UpdateReticule(ChipSO chip)
    {
        if(chip.RangeOfInfluence.Count == 0)
        {
            foreach(GameObject reticule in AimingReticules)
            {
                reticule.SetActive(false);
            }
            return;
        }

        foreach(GameObject reticule in AimingReticules)
        {
            reticule.SetActive(false);
        }        

        for(int i = 0; i < chip.RangeOfInfluence.Count; i++) 
        {
            AimingReticules[i].transform.localPosition = new Vector3(chip.RangeOfInfluence[i].x * 1.6f, chip.RangeOfInfluence[i].y, 0);
            AimingReticules[i].SetActive(true);

        }

    }


}
