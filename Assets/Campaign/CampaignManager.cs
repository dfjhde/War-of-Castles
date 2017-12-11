using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CampaignManager : MonoBehaviour
{
    public string AllMapCode;
    string[] mapcodeList;

    public GameObject BtnLevelPre;
    public Transform levelsPanel;

    public RectTransform mapPanel;

    public Button BtnBackToMain;
    public Button BtnBackToBig;
    public Island[] IslandList;

    public CrossList crossList;

    public int currentIslandId;


    [Serializable]
    public struct CrossList
    {
        public Button[] btn;
        public Image[] img;
    }

    [Serializable]
    public struct Island
    {
        public GameObject canvas;
        public RectTransform panel;
        public bool isScaleHeight;
        public Image img;
        public Button button;
    }


    void Start ()
    {
        SetUp();
        OnBtnBackToBigMap();
    }

    private void SetUp()
    {
        IslandList[2].button.interactable = Data.inst.GetLevelStage(19) == LevelStage.Won;
        IslandList[3].button.interactable = Data.inst.GetLevelStage(39) == LevelStage.Won;
        IslandList[4].button.interactable = Data.inst.GetLevelStage(49) == LevelStage.Won;
    }

    public void OnBtnIsland(int newIslandId)
    {
        Debug.Log("island "+ newIslandId);
        SetMap(newIslandId);

        for (int i = 20 * (newIslandId-1); i < 20 * (newIslandId); i++)
        {
            switch(Data.inst.GetLevelStage(i))
            {
                case LevelStage.Available:
                    crossList.img[i].color = Color.white;
                    crossList.btn[i].interactable = true;
                    break;

                case LevelStage.Won:
                    crossList.img[i].color = Color.red;
                    crossList.btn[i].interactable = true;
                    break;

                case LevelStage.Unavailable:
                    crossList.img[i].color = Color.gray;
                    //crossList.img[i].gameObject.SetActive(false);
                    crossList.btn[i].interactable = false;
                    break;
            }
        }
    }

    public void OnBtnBackToBigMap()
    {
        SetMap(0);
        //BtnBackToBig.gameObject.SetActive(false);
        //BtnBackToMain.gameObject.SetActive(true);
    }

    public void SetMap(int newIslandId)
    {
        IslandList[currentIslandId].canvas.SetActive(false);
        currentIslandId = newIslandId;
        IslandList[currentIslandId].canvas.SetActive(true);

        Sprite sprite = IslandList[currentIslandId].img.sprite;
        float radio = (float)sprite.rect.width / (float)sprite.rect.height;

        if (IslandList[currentIslandId].isScaleHeight)
        {
            IslandList[currentIslandId].panel.sizeDelta = new Vector2(radio * Screen.height, Screen.height);
        }
        else
        {
            IslandList[currentIslandId].panel.sizeDelta = new Vector2(Screen.width, (1/radio) * Screen.width);
        }
    }

    public void onBtnBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }


    public void OnBtnLevel(int mapID)
    {
        Data.inst.SetCurrentMap(mapID + 20 * (currentIslandId - 1), GameMode.Campaign);
        UnityEngine.SceneManagement.SceneManager.LoadScene("game3D");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("play scene");
    }
}
