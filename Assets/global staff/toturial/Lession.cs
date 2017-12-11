using System.Collections;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Lession")]
public class Lession : ScriptableObject
{
    public Step[] steps;
}

[Serializable]
public class Step
{
    public string descriptionString;
    public bool isNextBtnShown;
    public bool needPointer;
    public Vector3 pointerPos;
    public bool needHand;
    public AnimationClip handClip;
}