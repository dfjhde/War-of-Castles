using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameManager3D : GameManager
{
    public GameObject armyPre;
    public GameObject cityPre;
    public GameObject MapObj;
    public static GameManager3D inst3D;
    public static float SCALE = 0.05f;

    // on game over staff

    public OverManager overManager;

    public GameObject panelStop;

    public GameObject[] environments;
    public GameObject targetObj;


    void Awake()
    {
        GameManager.inst = this;
        GameManager3D.inst3D = this;
    }

    void Start()
    {
        OnStartGame();
        InstEnvironment();
    }

    public void InstEnvironment()
    {
        int id = Data.inst.GetCurrentMapInfo().id % environments.Length;
        Instantiate(environments[id]);
    }

    void Update()
    {
        OnFrameUpdate();
    }

    public override void onWin()
    {
        isOver = true;
        Data.inst.SetLevelStage(true);//record win or loss in local game
        overManager.OnOver(true);
    }

    public override void onLoss()
    {
        isOver = true;
        Data.inst.SetLevelStage(false);//record win or loss in local game
        overManager.OnOver(false);
    }

    public override Army InstantiateArmy(City fromCity)
    {
        GameObject reinObj = (GameObject)GameObject.Instantiate(armyPre, fromCity.transform.position, transform.localRotation);
        return (Army)reinObj.GetComponent<Army3D>();
    }

    public override City InstantiateCityObj(BuildingInfo buildingInfo)
    {
        GameObject bs = Instantiate(cityPre) as GameObject;
        bs.transform.SetParent(MapObj.transform);
        return bs.GetComponent<City3D>();
    }

    public static Vector3 XYtoVector3(int x, int y)
    {
        return new Vector3(x * SCALE, 0, y * SCALE);
    }

    public static Vector2 Vector3ToXY(Vector3 pos)
    {
        return new Vector2(pos.x / SCALE, pos.z / SCALE);
    }

    public override void ShowTargetObj(City city)
    {
        targetObj.transform.position = city.transform.position;
    }

    public override void HideTargetObj()
    {
        targetObj.transform.position = Vector3.down * 10;
    }

    public void onBtnRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("game3D");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("play scene");
    }

    public void onBtnBackTotitle()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }

    public void onBtnBackToCampaign()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Campaign");
    }

    public override bool isMouseDown()
    {
        return Input.GetButtonDown("Fire1");
    }

    public override bool isMouseUp()
    {
        return Input.GetButtonUp("Fire1");
    }

    public override void Stop()
    {
        base.Stop();
        panelStop.SetActive(true);
    }

    public override void Continue()
    {
        base.Continue();
        panelStop.SetActive(false);
    }
}
