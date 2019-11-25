using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using Cawotte.Toolbox.Audio;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public void Start()
    {
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void LoadMainLevel()
    {
        SceneManager.LoadScene("Assets/Scenes/MainScene.unity");
        AudioManager.Instance.Player.InterruptSound("MainMenu");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Assets/Scenes/MainMenu.unity");
        AudioManager.Instance.PlayMusic("MainMenu");
    }
}
