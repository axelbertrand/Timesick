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
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadBriefing()
    {
        SceneManager.LoadScene("Assets/Scenes/Main Game/Briefing.unity");
        AudioManager.Instance.Player.InterruptSound("MainMenu");
    }

    public void LoadMainLevel()
    {
        LevelTime = 0f;
        SceneManager.LoadScene("Assets/Scenes/Main Game/MainScene.unity");
        AudioManager.Instance.Player.InterruptSound("MainMenu");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Assets/Scenes/Main Game/MainMenu.unity");
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void LoadDebriefing()
    {
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
