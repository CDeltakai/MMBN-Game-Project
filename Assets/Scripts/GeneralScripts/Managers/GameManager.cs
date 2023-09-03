using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool BlockPausing = false;

    //Static player reference such that all classes have easy access to it
    [SerializeField] PlayerMovement PlayerReference; //Must be set in inspector
    public static PlayerMovement player{get; private set;}

    private void Awake()
    {
        player = PlayerReference;    
    }


    public static void PauseGame()
    {
        if(BlockPausing){return;}

        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
    }




}
