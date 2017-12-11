using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(menuName = "ToturialData")]
public class ToturialData : ScriptableObject
{
    public Lession[] lessions;

    public Lession GetLession(int levelID)
    {
        return lessions[levelID];
    }

    public bool haveLession(int levelID)
    {
        Debug.Log(lessions.Length + " " + levelID + " "+ (lessions.Length < levelID));
        return lessions.Length > levelID && lessions[levelID] != null;
    }
}


