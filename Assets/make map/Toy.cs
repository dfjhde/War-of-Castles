using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour
{
    public BuildingInfo info;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public void SetUp (BuildingInfo info, Transform parent)
    {
        this.info = info;
        this.transform.position = GameManager3D.XYtoVector3(info.x, info.y);
        this.meshRenderer.materials[1].color = Data.inst.colorList[info.colorCode];
        this.meshFilter.mesh = Data.inst.cityInfos[info.buildingCode].mesh;
        this.transform.SetParent(parent);
    }

}
