using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DebugButtons : MonoBehaviour
{
    TimeManager timeManager;
    [SerializeField] PlayerAttributeSO playerStats;

    private void Awake() {
        timeManager = FindObjectOfType<TimeManager>();
    }

    public void ExitApplication()
    {
        print("Quitting application");

        Application.Quit();
        
    }
    public void ResetScene()
    {
        playerStats.SaveToJson();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        ChipSelectScreenMovement.GameIsPaused = false;
        Time.timeScale = 1;

    }


}
