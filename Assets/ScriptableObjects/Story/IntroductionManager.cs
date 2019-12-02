using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cawotte.Toolbox;


public class IntroductionManager : MonoBehaviour
{
    [System.Serializable]
    public struct Scene
    {
        [TextArea]
        public string[] lines;

        public string ambiantSoundName; 
    }

    [SerializeField]
    public Scene[] scenes;

    private Controls controls;

    private int currentSceneIndex;
    private int currentLineIndex;

    private IEnumerator printCoroutine;

    public TextMeshProUGUI textBox;

    private int coroutineFrames;

    private void Start()
    {
        currentSceneIndex = 0;
        currentLineIndex = 0;

        coroutineFrames = 0;
    }

    private void Awake()
    {
        controls = new Controls();
        controls.Introduction.SkipIntro.performed += _ => EndIntroduction();
        controls.Introduction.ContinueIntro.performed += _ => ContinueIntroduction();

        printCoroutine = TypeLine(scenes[currentSceneIndex].lines[currentLineIndex]);
        StartCoroutine(printCoroutine);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void FixedUpdate()
    {
        coroutineFrames++;
        if (coroutineFrames > 3)
        {
            coroutineFrames = 0;
        }
    }

    IEnumerator TypeLine(string sentence)
    {
        textBox.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitUntil(() => coroutineFrames >= 3);
        }

        textBox.text += " ▼";
        yield return null;
    }

    public void EndIntroduction()
    {
        GameManager.Instance.LoadBriefing();
    }

    public void ContinueIntroduction()
    {
        StopCoroutine(printCoroutine);

        currentLineIndex++;

        if (currentLineIndex >= scenes[currentSceneIndex].lines.Length)
        {
            currentSceneIndex++;
            currentLineIndex = 0;
        }

        if (currentSceneIndex >= scenes.Length)
        {
            EndIntroduction();
        }

        printCoroutine = TypeLine(scenes[currentSceneIndex].lines[currentLineIndex]);
        StartCoroutine(printCoroutine);
    }
}
