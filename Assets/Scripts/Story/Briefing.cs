using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Briefing", menuName = "ScriptableObjects/Dialogues/Briefing")]
public class Briefing : ScriptableObject
{
    public Sprite image;

    [TextArea]
    public string text;
}
