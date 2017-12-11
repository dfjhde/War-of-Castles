using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapBuilder : MonoBehaviour 
{
    public Transform buildingList;
    public GameObject preToy;
    List<BuildingInfo> BuildingInfos;

    public void BuildMap2D(string mapCode, Transform buildingList)
    {
        BuildMap(mapCode, buildingList);

        foreach (BuildingInfo info in BuildingInfos)
        {
            BuildCastle2D(info);
        }
    }

    public void BuildMap3D(string mapCode, Transform buildingList)
    {
        BuildMap(mapCode, buildingList);

        foreach (BuildingInfo info in BuildingInfos)
        {
            BuildCastle3D(info);
        }
    }

    private void BuildMap(string mapCode, Transform buildingList)
    {
        this.buildingList = buildingList;
        //清除之前的东西
        Clear();

        //建筑列表部分
        BuildingInfos = Data.inst.UnfoldString(mapCode);
    }

    public void BuildCastle3D(BuildingInfo info)
    {
        Toy toy = JimUI.instObj<Toy>(preToy);
        toy.SetUp(info, buildingList);
    }

    public void BuildCastle2D(BuildingInfo info)
    {
        GameObject a = Instantiate(preToy) as GameObject;
        a.GetComponent<SpriteRenderer>().sprite = Data.inst.topList[info.buildingCode];
        a.GetComponent<SpriteRenderer>().color = Data.inst.colorList[info.colorCode];
        a.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Data.inst.backList[info.buildingCode];
        a.transform.SetParent(buildingList);
        a.transform.localPosition = GameManager2D.XYtoVector2(info.x, info.y);
        a.transform.localScale = Vector3.one * Data.inst.scale;
    }

    public void Clear()
    {
        foreach (Transform t in buildingList)
        {
            Destroy(t.gameObject);
        }
    }
}
