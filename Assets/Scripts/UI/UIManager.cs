using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    public Timer timer;
    public StaminaBar staminaBar;
    public GameObject componentsContainer;

    private void Start()
    {
        timer.RunTimer();
        componentsContainer.SetActive(true);
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

    public void UpdateBar(int newValue)
    {
        staminaBar.UpdateValue(newValue);
    }

    public void HideUI()
    {
        componentsContainer.SetActive(false);
        timer.PauseTimer();
    }
}
