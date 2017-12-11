using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum GameMode { Campaign, MakeMap, Challenge, Testing, PvP };
public enum LevelStage { Unavailable = '0', Available = '1', Won = '2' };
public enum Team { White, Red, Blue, Yellow, Purple };

public class Data : MonoBehaviour
{
    public static Data inst;
    public bool isTesting;
    public int testingLevel;
    public Language language;

    //2D
    public Sprite[] backList;
    public Sprite[] topList;

    //3D
    [SerializeField]
    public CityInfo[] cityInfos;

    public Color[] colorList;

    public float scale;

    public List<MapInfo> mapInfoList;

    [SerializeField]
    private MapInfo currentMapInfo;
    public GameMode gameMode;

    public string lastScene;

    private char[] record;

    void Awake()
    {
        //PlayerPrefs.SetString("allMap", "
        //PlayerPrefs.SetString("record", "10000000000000000000000000000000000000000000000000000000000000000000000000000000");
        if (Data.inst != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            inst = this;
        }
        DontDestroyOnLoad(this);
        if (!PlayerPrefs.HasKey("setting"))
        {
            PlayerPrefs.SetString("allMap", "0;0,1,0,0/2,1,-204,14/0,3,237,-27/'1;0,1,-249,67,/0,1,-416,-20,/0,1,-265,-199,/0,1,-50,-64,/0,3,173,78,/0,3,308,-59,/0,3,169,-216,/'2;0,1,-205,122,/0,1,-455,-18,/0,1,-240,-189,/0,0,-104,-44,/0,0,120,-55,/0,3,239,128,/0,3,426,-59,/0,3,204,-238,/'3;1,0,-15,-16,/0,1,-185,125,/0,1,-361,-29,/0,1,-226,-188,/0,3,191,-239,/0,3,328,-56,/0,3,170,135,/'4;2,0,-56,-34,/1,0,-262,-44,/1,0,176,-42,/0,1,-548,19,/0,1,-494,167,/0,1,-348,223,/0,1,-189,231,/0,3,189,-250,/0,3,337,-214,/0,3,451,-113,/0,3,487,37,/'5;1,1,-50,-11,/0,1,-201,-18,/0,1,129,-22,/0,3,381,67,/0,3,383,-156,/0,2,-387,119,/0,2,-409,-130,/'6;2,1,-82,12,/0,3,-254,220,/0,3,153,214,/0,3,-298,-226,/'7;0,1,-543,140,/0,1,-548,-34,/0,1,-557,-206,/0,3,557,157,/0,3,548,0,/0,3,548,-213,/0,0,-365,90,/0,0,-396,-126,/0,0,415,76,/0,0,415,-131,/1,0,-15,-28,/0,0,-216,-45,/0,0,-133,197,/0,0,-123,-262,/0,0,265,-28,/0,0,141,178,/0,0,130,-221,/'8;1,1,-351,-223,/1,3,305,-231,/1,2,304,240,/1,4,-342,238,/0,0,-279,-30,/0,0,44,-150,/0,0,176,90,/0,0,-134,190,/2,0,-75,15,/'9;0,1,-508,-262,/0,1,-228,-92,/1,0,-60,3,/0,3,101,-80,/0,2,95,130,/0,4,-209,104,/0,4,-370,209,/0,2,332,260,/0,3,306,-235,/'10;2,1,-546,-29,/1,1,-365,-216,/1,1,-353,119,/0,1,-163,250,/0,1,-167,-23,/0,1,-168,-245,/2,3,469,-13,/1,3,289,-169,/1,3,298,118,/0,3,80,255,/0,3,72,-28,/0,3,65,-242,/'11;0,1,-529,-241,/0,3,536,-258,/0,2,564,261,/0,4,-559,258,/0,0,-551,77,/0,0,-549,-88,/0,0,-446,-6,/0,0,-370,117,/0,0,-361,-126,/0,0,-263,-24,/0,0,-157,90,/0,0,-138,-143,/0,0,-55,-24,/0,0,62,99,/0,0,81,-144,/0,0,541,84,/0,0,543,-100,/0,0,433,-7,/0,0,318,102,/0,0,322,-145,/0,0,197,-11,/0,0,-263,-241,/0,0,-32,-242,/0,0,202,-239,/0,0,-257,229,/0,0,-41,238,/0,0,183,231,/'12;2,1,-480,-190,/2,3,424,-204,/2,2,-42,216,/0,2,-109,52,/0,2,82,60,/0,3,213,-237,/0,3,266,-86,/0,1,-297,-85,/0,1,-220,-217,/'");
            PlayerPrefs.SetString("record", "10000000000000000000000000000000000000000000000000000000000000000000000000000000");
            PlayerPrefs.SetString("setting", "5/20/10/30/15/40/");
            updateData();
        }

        LoadMapData();
        LoadRecordData();

        //language
        language = new Language();
        language.SetUp();

        //------------for Testing-----------
        if (isTesting) { SetCurrentMap(testingLevel, GameMode.Testing); }
        
        //------------for Testing-----------
    }

    public void SetCurrentMap(List<BuildingInfo> buildingInfoList, GameMode mode)
    {
        currentMapInfo = new MapInfo(buildingInfoList);
        this.gameMode = mode;
    }

    public void SetCurrentMap(MapInfo mapInfo, GameMode mode)
    {
        currentMapInfo = mapInfo;
        this.gameMode = mode;
    }

    public void SetCurrentMap(int mapID, GameMode mode)
    {
        currentMapInfo = mapInfoList[mapID];
        this.gameMode = mode;
    }

    public List<BuildingInfo> GetCurrentBuildingInfoList()
    {
        return currentMapInfo.GetbiList();
    }

    public MapInfo GetCurrentMapInfo()
    {
        return currentMapInfo;
    }

    public void updateData()
    {
        string a = PlayerPrefs.GetString("setting");
        string[] alist = a.Split(new string[] { "/" }, System.StringSplitOptions.None);
        cityInfos[0].startPop = int.Parse(alist[0]);
        cityInfos[1].startPop = int.Parse(alist[2]);
        cityInfos[2].startPop = int.Parse(alist[4]);
        cityInfos[0].topPop = int.Parse(alist[1]);
        cityInfos[1].topPop = int.Parse(alist[3]);
        cityInfos[2].topPop = int.Parse(alist[5]);
    }

    public List<BuildingInfo> UnfoldString(string mapCode)
    {
        string[] all = mapCode.Split(new string[] { "/" }, System.StringSplitOptions.None);

        List<BuildingInfo> biList = new List<BuildingInfo>();

        for (int i = 0; i < all.Length - 1; i++)//最后一个为空，所以是length－1
        {
            string[] a = all[i].Split(new string[] { "," }, System.StringSplitOptions.None);
            BuildingInfo bi = new BuildingInfo(int.Parse(a[0]), int.Parse(a[1]), int.Parse(a[2]), int.Parse(a[3]));
            biList.Add(bi);
        }
        return biList;
    }

    private void LoadMapData()
    {
        mapInfoList = new List<MapInfo>();
        string AllMapCode = PlayerPrefs.GetString("allMap", null); //Debug.Log(AllMapCode);
        string[] mapcodeList = AllMapCode.Split(new string[] { "'" }, System.StringSplitOptions.None);//Debug.Log(mapcodeList.Length);
        for (int i = 0; i < mapcodeList.Length - 1; i++)//最后一个为空，所以是length－1
        {
            string[] mapDetails = mapcodeList[i].Split(new string[] { ";" }, System.StringSplitOptions.None);
            mapInfoList.Add(new MapInfo(int.Parse(mapDetails[0]), mapDetails[0], UnfoldString(mapDetails[1])));
        }
    }

    private void LoadRecordData()
    {
        string recordString = PlayerPrefs.GetString("record", null);
        record = recordString.ToCharArray();
        //Debug.Log(recordString);
    }

    public void SetLevelStage(bool iswin)
    {
        if (iswin)
        {
            record[currentMapInfo.id] = (char)LevelStage.Won;
            if (GetLevelStage(currentMapInfo.id + 1) == LevelStage.Unavailable)
            {
                record[currentMapInfo.id + 1] = (char)LevelStage.Available;
            }
        }
        string recordString = new string(record);
        PlayerPrefs.SetString("record", recordString);
    }

    public LevelStage GetLevelStage(int levelId)
    {
        return (LevelStage)record[levelId];
    }

    public Team GetNeutralTeam()
    {
        return Team.White;
    }

    public Team GetPlayerTeam()
    {
        return Team.Red;
    }
}

[Serializable]
public class BuildingInfo
{
    public int buildingCode, colorCode;
    public int x, y;

    public BuildingInfo(int bc, int cc, int x, int y)
    {
        this.x = x;
        this.y = y;
        buildingCode = bc;
        colorCode = cc;
    }


    public BuildingInfo(int bc, int cc, Vector3 postion)
    {
        this.x = Mathf.RoundToInt(postion.x * 100f);
        this.y = Mathf.RoundToInt(postion.y * 100f);
        buildingCode = bc;
        colorCode = cc;
    }

    public override string ToString()
    {
        string a = "";
        a += buildingCode + ",";
        a += colorCode + ",";
        a += x + ",";
        a += y;
        return a;
    }


}

[Serializable]
public class MapInfo
{
    public int id;
    public string name;
    public string userName;
    private List<BuildingInfo> biList;
    public string mapCode;

    public MapInfo(int id, string name, List<BuildingInfo> biList)
    {
        this.biList = biList;
        this.id = id;
        this.name = name;
    }

    public MapInfo(int id, string name, string mapCode)
    {
        this.mapCode = mapCode;
        this.id = id;
        this.name = name;
    }

    public MapInfo(List<BuildingInfo> biList)
    {
        this.biList = biList;
    }

    public List<BuildingInfo> GetbiList()
    {
        if (biList != null)
        {
            return this.biList;
        }
        else
        {
            return Data.inst.UnfoldString(mapCode);
        }

    }

    public string GetMapCode()
    {
        if (mapCode != null && !mapCode.Equals(""))
        {
            return mapCode;
        }
        else
        {
            string a = "";
            foreach (BuildingInfo bi in biList)
            {
                a += bi.ToString() + "/";
            }
            return a;
        }
    }


    public override string ToString()
    {
        return name + ';' + mapCode;
    }

    // public static void 
}

[Serializable]
public struct CityInfo
{
    public Mesh mesh;
    public int topPop;
    public int startPop;
    public Vector2 bc2dOffset;
    public Vector2 bc2dSize;
    public Vector3 flagPos;
}

