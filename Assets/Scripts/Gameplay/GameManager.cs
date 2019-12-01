using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using Cawotte.Toolbox.Audio;
using UnityEngine.SceneManagement;
using System.Linq;


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

    [SerializeField]
    private List<SceneData> scenes = new List<SceneData>();

    public enum GameState
    {
        MAIN_MENU,
        INTRO,
        BRIEFING,
        LEVEL,
        DEBRIEF
    }

    [System.Serializable]
    public struct SceneData
    {
        public SceneData(GameState state, string scene, string musicName) : this(state, scene)
        {
            this.MusicName = musicName;
        }

        public SceneData(GameState state, string scene)
        {
            this.State = state;
            this.SceneName = scene;
            this.MusicName = "";
        }

        public GameState State;
        public string SceneName;
        public string MusicName;
    }

    private GameState currentState;

    public GameState CurrentState
    {
        get => currentState;
        set
        {
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
        SceneData currentScene = GetCurrentScene();
        currentState = currentScene.State;
        if (!currentScene.MusicName.Equals(""))
            AudioManager.Instance.PlayMusic(currentScene.MusicName);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadBriefing()
    {
        LoadSceneData(GameState.BRIEFING);
    }

    public void LoadMainLevel()
    {
        LoadSceneData(GameState.LEVEL);
        AudioManager.Instance.PlayMusic(FindScene(GameState.LEVEL).MusicName, true);
        LevelTime = 0f;
    }

    public void LoadMainMenu()
    {
        LoadSceneData(GameState.MAIN_MENU);
    }

    public void LoadIntroduction()
    {
        LoadSceneData(GameState.INTRO);
    }

    public void LoadDebriefing()
    {
        LoadSceneData(GameState.DEBRIEF);
        AudioManager.Instance.StopMusic();
    }

    public void OnLevelEnd()
    {
        LoadDebriefing();
    }

    public void OnDeath()
    {
        UIManager.Instance.ShowGameOver();
    }

    public void OnEscape()
    {
        UIManager.Instance.ShowEscaped();
    }

    /// <summary>
    /// Return the sceneData with the given GameState
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private SceneData FindScene(GameState state)
    {
        return scenes.Where(scene => scene.State.Equals(state)).First();
    }

    private SceneData GetCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        return scenes.Where(scene => scene.SceneName.Equals(currentSceneName)).First();
    }


    private void LoadSceneData(GameState state)
    {
        LoadSceneData(FindScene(state));
    }

    private void LoadSceneData(SceneData scene)
    {
        Debug.Log("Loading Scene : " + scene.SceneName);
        CurrentState = scene.State;
        SceneManager.LoadScene(scene.SceneName);
        if (scene.MusicName != "")
        {
            AudioManager.Instance.PlayMusic(scene.MusicName);
        }

    }
}
