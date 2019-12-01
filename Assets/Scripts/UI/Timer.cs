using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText;

    private float startTime;
    private float duration = 0f;
    private float currentTime = 0f;
    private bool run = false;

    public System.Action OnTimerEnd = null;

    public void StartTimer(float duration)
    {
        this.duration = duration;
        this.currentTime = duration;
        run = true;
    }
    public void RunTimer()
    {
        run = true;
    }

    public void ResetTimer()
    {
        RunTimer();
    }

    public void PauseTimer()
    {
        run = false;
    }

    public void ResumeTimer()
    {
        run = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(run)
        {
            currentTime -= Time.deltaTime;

            if (currentTime < 0f)
            {
                currentTime = 0f;
            }

            string minutes = ((int)(currentTime / 60)).ToString();
            if(minutes.Length < 2)
            {
                minutes = '0' + minutes;
            }
            string seconds = (currentTime % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;

            if (currentTime <= 0f)
            {
                GameManager.Instance.OnDeath("Game Over : Your time ran out!");
            }
        }
    } 
}
