using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debriefing", menuName = "ScriptableObjects/Dialogues/Debriefing")]
public class Debriefing : ScriptableObject
{
    [TextArea]
    public string text;
}
