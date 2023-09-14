using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    //If true, prevents the game from pausing.
    public static bool BlockPausing = false;

    [SerializeField] PlayerMovement PlayerReference; //Must be set in inspector
    //Static player reference such that all classes have easy access to it
    public static PlayerMovement player{get; private set;}

    private void Awake()
    {
        player = PlayerReference;    
    }


    public static void PauseGame()
    {
        if(BlockPausing){return;}
        print("Paused game");
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public static void UnpauseGame()
    {
        print("Unpaused game");
        Time.timeScale = 1;
        GameIsPaused = false;
    }




}
