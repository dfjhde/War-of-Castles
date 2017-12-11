using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameManager2D : GameManager 
{
	public GameObject armyPre;
	public GameObject cityPre;
	public GameObject torPre;

	public GameObject MapObj;
    public static float SCALE = 100f;
    // on game over staff
    public GameObject overPanelGo;
    public OverManager overManager;
    public Text overText;


	void Awake()  
	{
		GameManager.inst=this;
	}

	void Start () 
	{
        OnStartGame();
    }

    void Update()
    {
        OnFrameUpdate();
    }


    public void onBtnRestart()
	{
        UnityEngine.SceneManagement.SceneManager.LoadScene("play scene");
	}

	public void onBtnBack()
	{
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
	}

	public override void onWin()
	{
		overPanelGo.SetActive(true);
		isOver = true;
		overText.text = "you Win!!";
        Data.inst.SetLevelStage(true);//record win or loss in local game
        overManager.OnOver(true);
    }

	public override void onLoss()
	{
		overPanelGo.SetActive(true);
		isOver = true;
		overText.text = "you Loss!!";
        Data.inst.SetLevelStage(false);//record win or loss in local game
        overManager.OnOver(false);
    }

    public static Vector2 XYtoVector2(int x, int y)
    {
        return new Vector2(x / SCALE, y / SCALE);
    }
    public static Vector2 Vector2ToXY(Vector2 pos)
    {
        return new Vector2(pos.x * SCALE, pos.y * SCALE);
    }

    public override Army InstantiateArmy(City fromCity)
    {
        GameObject reinObj = (GameObject)GameObject.Instantiate(armyPre, fromCity.transform.position, transform.localRotation);
        reinObj.transform.localScale *= Data.inst.scale;
        return (Army)reinObj.GetComponent<Army2D>();
    }

    public override City InstantiateCityObj(BuildingInfo buildingInfo)
    {
        GameObject bs = Instantiate(cityPre) as GameObject;
        bs.transform.SetParent(MapObj.transform);
        bs.transform.position = XYtoVector2(buildingInfo.x, buildingInfo.y);
        bs.transform.localScale = Vector3.one * Data.inst.scale;
        return bs.GetComponent<City>();
    }

    public override bool isMouseDown()
    {
        return Input.GetButtonDown("Fire1");
    }

    public override bool isMouseUp()
    {
        return Input.GetButtonUp("Fire1");
    }

    public override void ShowTargetObj(City city)
    {
        
    }

    public override void HideTargetObj()
    {
        
    }
}


public abstract class GameManager : MonoBehaviour
{
    public bool choosing = false;

    public bool isPlaying = false;
    //public bool isCan = true;
    public float gameSpeed = 1;
    public static float ARMY_SPEED = 50f;//Army Moving per sec
    public static int TOP_RATE = 4;

    public Team playerTeam;

    public bool isOver;

    public List<City> allCities;
    public List<Army> allArmies;
    public static GameManager inst;
    public City MyTargetCity;
    public List<City> MyFromCities;
    public List<AiBase> AIs;// = new List<AiBase>();




    protected void OnStartGame()
    {
        GenerateMap();
        startAi();
        MyFromCities = new List<City>();
        SetIsPlaying(true);
    }

    protected virtual void SetIsPlaying(bool isPlaying)
    {
        this.isPlaying = isPlaying;
    }

    protected virtual void OnFrameUpdate()
    {
        if (!isPlaying)
        {
            return;
        }
        if (isMouseDown())
        {
            choosing = true;
        }

        if (isMouseUp())
        {
            choosing = false;
            if (MyTargetCity != null)
            {
                BuildFroce(MyFromCities, MyTargetCity);
                if (Toturial.inst != null) { Toturial.inst.OnPlayerMoved(); }
            }
            MyFromCities.Clear();
        }

        if (!isOver)
        {
            CheckState();
        }
    }

    protected void startAi()
    {
        foreach (AiBase ai in AIs)
        {
            ai.OnGameStart();
        }
    }

    // check if anyone win
    public void CheckState()
    {
        int playerArmyCount = 0;
        foreach (Army army in allArmies)
        {
            if (army.GetTeam() == playerTeam)
            {
                playerArmyCount++;
            }
        }

        int playerCityCount = 0;
        int NeutralCityCount = 0;
        foreach (City city in allCities)
        {
            if (city.IsSameTeam(playerTeam))
            {
                playerCityCount++;
            }
            else if (city.IsNeutral())
            {
                NeutralCityCount++;
            }
        }

        if (playerArmyCount == 0 && playerCityCount == 0)
        {
            //Debug.Log(playerCityCount + " "+ playerTeam);
            onLoss();
        }
        else if (playerArmyCount == allArmies.Count && (playerCityCount + NeutralCityCount) == allCities.Count)
        {
            onWin();
        }
    }

    public virtual void BuildFroce(List<City> fromCities, City targetCity)
    {
        //------------检查参数是否有效--------------
        if (fromCities.Count == 0 || targetCity == null)
        {
            return;
        }
        //------------检查参数是否有效--------------

        foreach (City city in fromCities)
        {
            BuildFroce(city, targetCity);
        }
    }

    public void BuildFroce(City fromCity, City targetCity)
    {
        //------------检查参数是否有效--------------
        if (fromCity == null || targetCity == null)
        {
            return;
        }
        //------------检查参数是否有效--------------

        int halfPop = fromCity.GetPopulation() / 2;
        fromCity.SetPopulation(halfPop);
        Army army = InstantiateArmy(fromCity);
        allArmies.Add(army);
        army.Setup(halfPop, fromCity.GetTeam(), fromCity, targetCity);
    }

    protected void GenerateMap()
    {
        List<BuildingInfo> biList = Data.inst.GetCurrentBuildingInfoList();

        foreach (BuildingInfo bi in biList)
        {
            City city = InstantiateCityObj(bi);
            city.SetUp(bi.x, bi.y, bi.buildingCode, (Team)bi.colorCode);
        }
    }

    public virtual void Stop()
    {
        SetIsPlaying(false);
    }

    public virtual void Continue()
    {
        SetIsPlaying(true);
    }

    public abstract void onWin();

    public abstract void onLoss();

    public abstract Army InstantiateArmy(City fromCity);

    public abstract bool isMouseDown();

    public abstract bool isMouseUp();

    public abstract City InstantiateCityObj(BuildingInfo buildingInfo);

    public abstract void ShowTargetObj(City city);

    public abstract void HideTargetObj();

}


