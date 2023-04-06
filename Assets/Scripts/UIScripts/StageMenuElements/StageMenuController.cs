using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageMenuController : MonoBehaviour
{

    [SerializeField] UnityEngine.GameObject pauseMenu;
    [SerializeField] UnityEngine.GameObject deckEditMenu;
    [SerializeField] UnityEngine.GameObject playerEditMenu;
    [SerializeField] GameObject stageSelectMenu;


    [SerializeField] UnityEngine.GameObject currentActiveMenu;

    bool StageMenuTriggered = false;
    bool PauseMenuTriggered = false;
    bool DeckEditMenuTriggered = false;
    bool PlayerEditMenuTriggered = false;


    private void Awake() 
    {
        currentActiveMenu = null;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Input action method - Normally tied to the ESC key when using the PlayerControl Input InputActionAsset
    public void EnablePauseMenu(InputAction.CallbackContext context)
    {
        if(TimeManager.isCurrentlySlowedDown)
        {
            return;
        }
                
        if(context.started)
        {
            //Open up the pause menu
            if(!StageMenuTriggered)
            {
                print("Opened pause menu");
                ChipSelectScreenMovement.GameIsPaused = true;
                Time.timeScale = 0;
                StageMenuTriggered = true;
                pauseMenu.SetActive(true);
                currentActiveMenu = pauseMenu;
                return;

            }


            //Exit out of pause menu and resume
            if(StageMenuTriggered && currentActiveMenu == pauseMenu)
            {
                Time.timeScale = 1;
                ChipSelectScreenMovement.GameIsPaused = false;                
                StageMenuTriggered = false;
                pauseMenu.SetActive(false);
                currentActiveMenu = null;
                return;
            }

            //If the a submenu like the DeckEditMenu or Option menu is open, pressing ESC will return to the pause menu
            //instead of returning immediately to the game.
            if(StageMenuTriggered && currentActiveMenu != pauseMenu)
            {

                currentActiveMenu.SetActive(false);
                currentActiveMenu = pauseMenu;
                pauseMenu.SetActive(true);
                return;
            }

        }

    }

    public void ResumeButton()
    {
        if(StageMenuTriggered && currentActiveMenu == pauseMenu)
        {
            Time.timeScale = 1;
            ChipSelectScreenMovement.GameIsPaused = false;                
            StageMenuTriggered = false;
            pauseMenu.SetActive(false);
            currentActiveMenu = null;
            return;
        }        
    }

    public void ReturnToPauseMenu()
    {
        if(StageMenuTriggered && currentActiveMenu != pauseMenu)
        {

            currentActiveMenu.SetActive(false);
            currentActiveMenu = pauseMenu;
            pauseMenu.SetActive(true);
            return;
        }        
    }

    public void OpenPauseMenuButton()
    {
        if(!StageMenuTriggered)
        {
            ChipSelectScreenMovement.GameIsPaused = true;
            Time.timeScale = 0;
            StageMenuTriggered = true;
            pauseMenu.SetActive(true);
            currentActiveMenu = pauseMenu;
            return;

        }        
    }

    public void EnableDeckEditMenu()
    {
        currentActiveMenu.SetActive(false);
        currentActiveMenu = deckEditMenu;
        deckEditMenu.SetActive(true);
    }

    public void EnableStageSelectMenu()
    {
        currentActiveMenu.SetActive(false);
        currentActiveMenu = stageSelectMenu;
        stageSelectMenu.SetActive(true);
    }



}
