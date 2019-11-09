using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText;

    private float startTime;
    private bool run = false;

    public void RunTimer()
    {
        startTime = Time.time;
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
            float t = Time.time - startTime;

            string minutes = ((int)(t / 60)).ToString();
            if(minutes.Length < 2)
            {
                minutes = '0' + minutes;
            }
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds;
        }
    } 
}
