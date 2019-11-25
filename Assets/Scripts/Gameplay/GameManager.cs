using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public void LoadMainLevel()
    {
        SceneManager.LoadScene("Assets/Scenes/MainScene.unity");
    }
}
