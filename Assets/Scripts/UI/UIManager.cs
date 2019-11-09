using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
public class UIManager : Singleton<UIManager>
{
    public Timer timer;
    public StaminaBar staminaBar;

    private void Start()
    {
        timer.RunTimer();
        staminaBar.Initialize();
    }

    public void RunTimer()
    {
        timer.RunTimer();
    }

    public void PauseTimer()
    {
        timer.PauseTimer();
    }

    public void ResumeTimer()
    {
        timer.ResumeTimer();
    }

    public void UpdateBar(int value)
    {
        staminaBar.UpdateValue(value);
    }
}
