using Cawotte.Toolbox.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Necessary Workaround
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    
    public void LoadIntroduction()
    {
        AudioManager.Instance.PlayClickUISound();
        GameManager.Instance.LoadIntroduction();
    }

    public void Quit()
    {
        AudioManager.Instance.PlayClickUISound();
        GameManager.Instance.Quit();
    }
}
