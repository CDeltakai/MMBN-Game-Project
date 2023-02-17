using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StageMenuController : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deckEditMenu;
    [SerializeField] GameObject playerEditMenu;


    [SerializeField] GameObject currentActiveMenu;

    bool StageMenuTriggered = false;
    bool PauseMenuTriggered = false;
    bool DeckEditMenuTriggered = false;
    bool PlayerEditMenuTriggered = false;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnablePauseMenu(InputAction.CallbackContext context)
    {
        if(context.started)
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

            if(StageMenuTriggered && currentActiveMenu != pauseMenu)
            {

                currentActiveMenu.SetActive(false);
                currentActiveMenu = pauseMenu;
                pauseMenu.SetActive(true);
                return;
            }





        }


    }




}
