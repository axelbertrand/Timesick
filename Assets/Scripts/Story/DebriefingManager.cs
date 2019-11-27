﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebriefingManager : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public TextMeshProUGUI scoreTextBox;

    public Debriefing debriefing;


    // Start is called before the first frame update
    void Start()
    {
        textBox.text = debriefing.text;

        scoreTextBox.text = string.Format("Your time is : {0]",GameManager.Instance.FormattedLevelTime);
    }

    public void StartLevel()
    {
        GameManager.Instance.LoadMainLevel();
    }

    public void ReturnToMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
