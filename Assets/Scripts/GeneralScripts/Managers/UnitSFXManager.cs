using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

[System.Serializable]
public class SFXReference
{
    [SerializeField] internal string SFXName;
    [SerializeField] internal EventReference SFX;


}



public class UnitSFXManager : MonoBehaviour
{

    [SerializeField] List<SFXReference> SFXList = new List<SFXReference>();
    Dictionary<string, EventReference> SFXDictionary = new Dictionary<string, EventReference>();

    private void Awake() 
    {
        FillSFXDictionary();
    }


    void Start()
    {

    }

    private void FillSFXDictionary()
    {
        foreach(SFXReference sfxref in SFXList)
        {
            SFXDictionary.Add(sfxref.SFXName, sfxref.SFX);
        }

    }



    public void PlaySFXByID(string ID)
    {
        EventReference SFXToPlay = SFXDictionary[ID];
        FMODUnity.RuntimeManager.PlayOneShotAttached(SFXToPlay, this.gameObject);
    }



    public void PlayChipSFX(ChipObjectReference chip)
    {
            if(!chip.chipSORef.GetSFX().IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShotAttached(chip.chipSORef.GetSFX(), this.gameObject);
            }else
            {
                Debug.LogWarning("Chip used does not have an EventReference for SFX");
            }    
    }


}
