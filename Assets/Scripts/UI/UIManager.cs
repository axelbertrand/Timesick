using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cawotte.Toolbox;
using UnityEngine.UI;
using Cawotte.Toolbox.Audio;
using TMPro;
using Sirenix.OdinInspector;

public class UIManager : Singleton<UIManager>
{

    [Title("Gameplay UI")]
    public GameObject componentsContainer;
    public Slider staminaBar;
    public Timer timer;
    public float durationTimer = 40f;

    public Image trapDisabledImage;
    public Image invisiblityDisabledImage;

    [Title("Game Over Screen")]
    public GameObject gameOverContainer;
    public TextMeshProUGUI gameOverText;

    [Title("Other")]
    public GameObject escapedContainer;
    public GameObject playerObject;

    private void Start()
    {
        timer.StartTimer(durationTimer);
        componentsContainer.SetActive(true);
        gameOverContainer.SetActive(false);
        escapedContainer.SetActive(false);
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

    public void ShowGameOver(string gameOverMessage)
    {
        gameOverText.text = gameOverMessage;
        playerObject.SetActive(false);
        HideUI();
        gameOverContainer.SetActive(true);
    }

    public void ShowEscaped()
    {
        playerObject.SetActive(false);
        HideUI();
        escapedContainer.SetActive(true);
    }

    public void SetTrapEnabled(bool enable)
    {
        trapDisabledImage.gameObject.SetActive(!enable);
    }

    public void SetInvisibilityEnabled(bool enable)
    {
        invisiblityDisabledImage.gameObject.SetActive(!enable);
    }

    public void OnClickQuit()
    {
        AudioManager.Instance.PlayClickUISound();
        GameManager.Instance.LoadMainMenu();
    }


    public void OnClickRestart()
    {
        AudioManager.Instance.PlayClickUISound();
        GameManager.Instance.LoadMainLevel();
    }
    public void OnClickDebrief()
    {
        AudioManager.Instance.PlayClickUISound();
        GameManager.Instance.LoadDebriefing();
    }
}
