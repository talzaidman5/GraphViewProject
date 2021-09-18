using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Serializable]
public class DialogueNode : Node
{
    public string GUID;
    public string DialogText;
    public bool EntryPoint = false;
    public Color inputColor;
    public Color outputColor;
}
