using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLoadManager : MonoBehaviour
{

public delegate void LoadChipsEvent();
public event LoadChipsEvent loadChipsEvent;

[SerializeField] public List<ChipSO> nextChipLoad = new List<ChipSO>();
[SerializeField] public List<ChipObjectReference> nextChipRefLoad = new List<ChipObjectReference>();


[SerializeField] public List<ChipSO> chipQueue = new List<ChipSO>();
[SerializeField] public List<ChipObjectReference> chipRefQueue = new List<ChipObjectReference>();


List<int> chipsToRemoveIndexes = new List<int>();
List<ChipSO> chipsToRemove = new List<ChipSO>();
List<ChipObjectReference> chipRefsToRemove = new List<ChipObjectReference>();


[SerializeField] public float damageAmp = 0;
[SerializeField] public int damageAdd = 0;
PlayerMovement player;



    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        
    }


    public void calcNextChipLoad()
    {

        if(chipQueue.Count == 0)
        {
            print("Chip Qeue Empty " + "Class: ChipLoadManager");
            return;
        }

        nextChipLoad.Add(chipQueue[0]);
        chipQueue.RemoveAt(0);

        foreach(ChipSO chip in chipQueue)
        {
            if(chip.GetChipType() == EChipTypes.Passive)
            {
                nextChipLoad.Add(chip);
                chipsToRemove.Add(chip);

            }else
            {
                break;
            }
        }

        foreach(ChipSO chip in chipsToRemove)
        {
            chipQueue.Remove(chip);
        }

        chipsToRemoveIndexes.Clear();
        if(loadChipsEvent != null)
        {loadChipsEvent();} 


    }

    public void calcNextChipRefLoad()
    {
        if(chipRefQueue.Count == 0)
        {
            print("ChipRef Qeue Empty " + "Class: ChipLoadManager");
            return;
        }

        nextChipRefLoad.Add(chipRefQueue[0]);
        chipRefQueue.RemoveAt(0);

        foreach(ChipObjectReference chipRef in chipRefQueue)
        {
            if(chipRef.chipSORef.GetChipType() == EChipTypes.Passive)
            {
                nextChipRefLoad.Add(chipRef);
                chipRefsToRemove.Add(chipRef);

            }else
            {
                break;
            }
        }

        foreach(ChipObjectReference chipRef in chipRefsToRemove)
        {
            chipRefQueue.Remove(chipRef);
        }

        if(loadChipsEvent != null)
        {print("Attempted loadChipsEvent");
            loadChipsEvent();} 


    }


}
