using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void SelectStage(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1;
        ChipSelectScreenMovement.GameIsPaused = false;           
    }


}
