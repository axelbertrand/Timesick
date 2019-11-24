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

    public void InitializeStaminaBar(int staminaMax, int currentStamina)
    {
        staminaBar.Initialize(staminaMax,currentStamina);
        //staminaBar.UpdateValue(currentStamina);
    }

    public void UpdateBar(int value)
    {
        staminaBar.UpdateValue(value);
    }
}
