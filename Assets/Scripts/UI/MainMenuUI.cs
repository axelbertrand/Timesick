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
        GameManager.Instance.LoadIntroduction();
    }

    public void Quit()
    {
        GameManager.Instance.Quit();
    }
}
