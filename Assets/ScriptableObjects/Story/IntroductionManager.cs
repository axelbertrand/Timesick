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
        printCoroutine = TypeLine(scenes[currentSceneIndex].lines[currentLineIndex]);
        StartCoroutine(printCoroutine);
    }

    public void Update()
    {
        if (InputManager.GetButtonDown(Button.SKIP_INTRO))
        {
            EndIntroduction();
        }

        if(InputManager.GetButtonDown(Button.CONTINUE_INTRO))
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
}
