using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Toturial : MonoBehaviour
{
    public static Toturial inst;
    public Animation hand;
    public Sprite tapOn;
    public Sprite tapOff;
    public Text description;
    public GameObject btnStop;
    public GameObject btnNext;
    public RectTransform pointer;

    int index;
    public ToturialData toturialData;
    public Lession lession;

    void Start()
    {
        inst = this;
        int levelID = Data.inst.GetCurrentMapInfo().id;
        if (!toturialData.haveLession(levelID) || Data.inst.gameMode != GameMode.Campaign)
        {
            End();
            return;
        }

        lession = toturialData.GetLession(levelID);
        index = -1;
        Next();
        GameManager.inst.Stop();
        GameManager3D.inst3D.panelStop.SetActive(false);
        btnStop.SetActive(false);
    }


    private void SetDescription(string des)
    {
        description.text = Data.inst.language.GetLanguage(des);
    }


    public void OnPlayerMoved()
    {
        Next();
    }

    public void Next()
    {
        index ++;

        if (index >= lession.steps.Length)
        {
            End();
            return;
        }

        Step step = lession.steps[index];

        SetDescription(step.descriptionString);
        btnNext.SetActive(step.isNextBtnShown);

        if (!step.isNextBtnShown)
        {
            GameManager.inst.Continue();
        }


        pointer.gameObject.SetActive(step.needPointer);
        pointer.localPosition = step.pointerPos;

        hand.gameObject.SetActive(step.needHand);
        if (step.needHand)
        {
            hand.clip = step.handClip;
            hand.Play();
        }
    }

    void End()
    {
        GameManager.inst.Continue();
        btnStop.SetActive(true);
        Destroy(this.gameObject);
    }

}