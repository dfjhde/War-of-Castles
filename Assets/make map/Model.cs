using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Model : MonoBehaviour
{
    public BuildingInfo info;
    public MapBuilder mapBuilder;

    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public BoxCollider2D bc2d;

	public bool isclear;
	public bool isChoosingAModel = false;

	void Start()
	{
        info = new BuildingInfo(0,0,0,0);
		isclear = true;
	}


	void Update () 
	{
		if(isChoosingAModel)
		{
            FollowMouse();
        }

        if (Input.GetMouseButtonUp(0) && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("makeMap"))
        {
            BuildCastle3D();
            this.gameObject.SetActive(false);
        }
    }



    public void FollowMouse()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        //Physics.Raycast(ray, )
        transform.position = hit.point;
        /*
        curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        CurPosition = MakeMapManager.inst.adjustCamera.ScreenToWorldPoint(curScreenSpace);
        CurPosition.z = 0;
        transform.position = CurPosition + Vector3.back;

        float x = Mathf.Round(CurPosition.x * 100f) / 100f;
        float y = Mathf.Round(CurPosition.y * 100f) / 100f;
        CurPosition = new Vector3(x, y, 0);
        

        if (CurPosition.x < 6f && CurPosition.x > -6f && CurPosition.y < 3.375f && CurPosition.y > -3.375f)
        {
            transform.position = CurPosition + Vector3.back;
        }
        */
    }

    public void BuildCastle3D()
	{
        Vector2 v2 = GameManager3D.Vector3ToXY(transform.position);
        info.x = (int)v2.x;
        info.y = (int)v2.y;

        mapBuilder.BuildCastle3D(info);

        BuildingInfo newInfo = new BuildingInfo(info.buildingCode, info.colorCode, info.x, info.y);
        MakeMapManager.inst.infoList.Add(newInfo);
        

        /*
		if(this.bc2d != null)
		{
			BoxCollider2D bc2d = a.transform.GetComponent<BoxCollider2D>();
			bc2d.offset   = this.bc2d.offset;
			bc2d.size     = this.bc2d.size;
		}
        */
    }



    public void ChangeModel(int buildingCode, Team team)
    {
        this.meshFilter.mesh = Data.inst.cityInfos[buildingCode].mesh;
        meshRenderer.materials[1].color = Data.inst.colorList[(int)team];
        info.colorCode = (int)team;
        info.buildingCode = buildingCode;
/*        
        topSr.sprite = newTopSr; 
        topSr.color = color;
        botSr.sprite = newBackSr;
        bc2d.offset = Data.inst.cityInfos[buildingCode].bc2dOffset;
        bc2d.size = Data.inst.cityInfos[buildingCode].bc2dSize;
        */
    }


    void OnTriggerEnter2D(Collider2D other) 
	{
		isclear = false;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		isclear = true;
	}
	void OnTriggerStay2D(Collider2D other)
	{
		isclear = false;
	}

}