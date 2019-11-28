using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BriefingManager : MonoBehaviour
{
    public Image image;
    public Briefing briefing;
    public TextMeshProUGUI textBox;


    // Start is called before the first frame update
    void Start()
    {
        //image.sprite = briefing.image;
        //textBox.text = briefing.text;
    }

    public void StartLevel()
    {
        GameManager.Instance.LoadMainLevel();
    }
   
}
