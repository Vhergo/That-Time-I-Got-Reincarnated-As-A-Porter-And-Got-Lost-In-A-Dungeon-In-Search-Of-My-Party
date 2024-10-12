using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string dialogueName;
    public string dialogueText;
    public Speaker speaker;
    public float dialogueDuration;
    public PlayableAsset cutscene;
}

[Serializable]
public enum Speaker
{
    Porter,
    Guard
}
