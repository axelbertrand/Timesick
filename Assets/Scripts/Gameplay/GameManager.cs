using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using Cawotte.Toolbox.Audio;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    private float levelTime;

    public float LevelTime
    {
        get => levelTime;
        set
        {
            levelTime = value;
        }
    }

    public enum GameState
    {
        MAIN_MENU,
        INTRO,
        BRIEFING,
        LEVEL,
        DEBRIEF
    }

    private GameState currentState;

    public GameState CurrentState
    {
        get => currentState;
        set
        {
            if(currentState == GameState.LEVEL)
            {
                AudioManager.Instance.Player.InterruptSound("AmbiantLoop");
            }

            currentState = value;
        }
    }

    public string FormattedLevelTime
    {
        get
        {
            string minutes = ((int)(levelTime / 60)).ToString();
            if (minutes.Length < 2)
            {
                minutes = '0' + minutes;
            }
            string seconds = (levelTime % 60).ToString("f2");

            return string.Format("{0]:{1}",minutes,seconds);
        }
    }

    public void Start()
    {
        currentState = GameState.MAIN_MENU;
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void Update()
    {
        if(CurrentState == GameState.LEVEL)
        {
            if(!AudioManager.Instance.Player.IsCurrentlyPlayed("AmbiantLoop"))
            {
                AudioManager.Instance.PlaySound("AmbiantLoop");
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadBriefing()
    {
        CurrentState = GameState.BRIEFING;
        AudioManager.Instance.Player.InterruptSound("MainMenu");
        SceneManager.LoadScene("Assets/Scenes/Main Game/Briefing.unity");
    }

    public void LoadMainLevel()
    {
        CurrentState = GameState.LEVEL;

        LevelTime = 0f;
        SceneManager.LoadScene("Assets/Scenes/Main Game/MainScene.unity");
    }

    public void LoadMainMenu()
    {
        CurrentState = GameState.MAIN_MENU;

        SceneManager.LoadScene("Assets/Scenes/Main Game/MainMenu.unity");
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void LoadIntroduction()
    {
        CurrentState = GameState.INTRO;
        AudioManager.Instance.Player.InterruptSound("MainMenu");
        SceneManager.LoadScene("Assets/Scenes/Main Game/Introduction.unity");
    }

    public void LoadDebriefing()
    {
        currentState = GameState.DEBRIEF;
        SceneManager.LoadScene("Assets/Scenes/Main Game/Debriefing.unity");
    }

    public void OnLevelEnd()
    {
        LoadDebriefing();
    }

    public void OnDeath()
    {
        UIManager.Instance.ShowGameOver();
    }
}
