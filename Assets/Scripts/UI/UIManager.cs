using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    public Timer timer;
    public Slider staminaBar;
    public GameObject componentsContainer;
    public GameObject gameOverContainer;
    public GameObject playerObject;

    private void Start()
    {
        timer.RunTimer();
        componentsContainer.SetActive(true);
        gameOverContainer.SetActive(false);
        playerObject.SetActive(true);
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
        staminaBar.maxValue = staminaMax;
        staminaBar.value = currentStamina;
    }

    public void UpdateBar(int newValue)
    {
        staminaBar.value = newValue;
    }

    public void HideUI()
    {
        componentsContainer.SetActive(false);
        timer.PauseTimer();
    }

    public void ShowGameOver()
    {
        playerObject.SetActive(false);
        HideUI();
        gameOverContainer.SetActive(true);
    }

    public void OnClickQuit()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
