using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DebugButtons : MonoBehaviour
{
    TimeManager timeManager;
    PlayerMovement player;
    [SerializeField] PlayerAttributeSO playerStats;
    [SerializeField] ObjectPoolManager objectPoolManager;

    private void Awake() {
        player = FindObjectOfType<PlayerMovement>();
        timeManager = FindObjectOfType<TimeManager>();
    }


    public void PlayerToggleInvincible()
    {
        player.ToggleInvincible();
    }

    public void ExitApplication()
    {
        print("Quitting application");

        Application.Quit();
        
    }
    public void ResetScene()
    {
        //playerStats.SaveToJson();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        ChipSelectScreenMovement.GameIsPaused = false;
        Time.timeScale = 1;

    }

    public void LoadSaveJsonUtility()
    {
        playerStats.LoadJsonUtilitySave();

    }

    public void LoadSaveNewtonsoft()
    {
        playerStats.LoadNewtonsoftSave();
    }

    public void ReloadDeck()
    {
        objectPoolManager.ReloadObjectPool();
    }

}
