using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool GameIsPaused = false;


    public static void PauseGame()
    {
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
    }




}
