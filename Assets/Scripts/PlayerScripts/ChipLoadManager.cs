using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipLoadManager : MonoBehaviour
{

[SerializeField] public List<ChipSO> nextChipLoad = new List<ChipSO>();
[SerializeField] public List<ChipSO> chipQueue = new List<ChipSO>();
List<int> chipsToRemoveIndexes = new List<int>();
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
                //chipQueue.Remove(chip);
                chipsToRemoveIndexes.Add(chipQueue.IndexOf(chip)); 


            }else
            {
                break;
            }
        }

        foreach(int index in chipsToRemoveIndexes)
        {
            chipQueue.RemoveAt(index);
        }
        chipsToRemoveIndexes.Clear();



    }



}
