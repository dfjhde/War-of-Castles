using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class JimUI : MonoBehaviour
{
    public static T instObj<T>(GameObject objPre)
    {
        GameObject g = Instantiate(objPre) as GameObject;
        return g.GetComponent<T>();
    }

    public static GameObject instUI(GameObject uiPre, RectTransform parent)
    {
        GameObject g = Instantiate(uiPre) as GameObject;
        g.transform.SetParent(parent);
        g.transform.localPosition = Vector3.zero;
        return g;
    }

    public static GameObject instUI(GameObject btnPre, RectTransform parent, string label)
    {
        GameObject g = instUI(btnPre, parent);
        g.GetComponentInChildren<Text>().text = label;
        return g;
    }

    public static GameObject instBtn<T>(GameObject btnPre, RectTransform parent, string label, Action<T> action, T premeter)
    {
        GameObject g = instUI(btnPre, parent, label);
        g.GetComponentInChildren<Button>().onClick.AddListener(delegate { action(premeter); });
        return g;
    }

    public static void SameFontSize(MonoBehaviour obj, List<Text> texts)
    {
        obj.StartCoroutine(setFont(texts));
    }

    private static IEnumerator setFont(List<Text> texts)
    {
        int minText = 100;

        while (texts[0].cachedTextGenerator.fontSizeUsedForBestFit == 0)
        { yield return new WaitForSeconds(0); }

        foreach (Text text in texts)
        {
            int size = text.cachedTextGenerator.fontSizeUsedForBestFit;
            Debug.Log(size);
            minText = minText > size ? size : minText;
        }

        foreach (Text text in texts)
        {
            text.resizeTextMaxSize = minText;
        }
        yield return null;
    }


}
