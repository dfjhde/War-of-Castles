using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MakeMapManager : MonoBehaviour
{
    public static MakeMapManager inst;
    public ModelBtn modelBtn1;
    public ModelBtn modelBtn2;
    public ModelBtn modelBtn3;
    public Model model;
    public Transform buildingList;
    public Camera adjustCamera;
    public JimText info;
    public RectTransform infoPanel;
    public List<BuildingInfo> infoList;
    public bool haveMyTeam = false;
    public bool haveEnemy = false;

    // Use this for initialization
    void Start ()
    {
        inst = this;
        infoPanel.gameObject.SetActive(false);
        infoList = new List<BuildingInfo>();

        OnBtnColor((int)Data.inst.GetPlayerTeam());
	}



    public void OnBtnColor(int i)
    {
        modelBtn1.meshRenderer.materials[1].color = Data.inst.colorList[i];
        modelBtn2.meshRenderer.materials[1].color = Data.inst.colorList[i];
        modelBtn3.meshRenderer.materials[1].color = Data.inst.colorList[i];

        modelBtn1.team = (Team)i;
        modelBtn2.team = (Team)i;
        modelBtn3.team = (Team)i;

    }

    public void OnBtnBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }


    public void SaveMap()
    {
        haveEnemy = infoList.Exists((info) => { return info.colorCode != (int)Data.inst.GetPlayerTeam() && info.colorCode != (int)Data.inst.GetNeutralTeam(); });
        haveMyTeam = infoList.Exists((info) => { return info.colorCode == (int)Data.inst.GetPlayerTeam(); });
        Data.inst.SetCurrentMap(infoList, GameMode.MakeMap);
        /*
        string a = "";
        foreach (Transform g in buildingList)
        {
            int bc = 0;
            int cc = 1;
            //建筑类型
            for (int i = 0; i < Data.inst.backList.Length; i++)
            {
                if (g.GetComponent<SpriteRenderer>().sprite == Data.inst.topList[i])
                {
                    bc = i;
                    break;
                }
            }

            for (int i = 0; i < Data.inst.colorList.Length; i++)
            {
                if (g.GetComponent<SpriteRenderer>().color == Data.inst.colorList[i])
                {
                    cc = i;
                    break;
                }
            }

            if (cc == (int)Data.inst.GetPlayerTeam())
            {
                haveMyTeam = true;
            } else if (cc != (int)Data.inst.GetPlayerTeam() && cc != (int)Data.inst.GetNeutralTeam())
            {
                haveEnemy = true;
            }

            BuildingInfo bi = new BuildingInfo(bc, cc, g.transform.position);
            a += bi.ToString()+"/";
            infoList.Add(bi);

        }
                    */

    }

    public void OnBtnPlay()
    {
        SaveMap();
        
        if (!haveEnemy)
        {
            info.SetText("You Need Some Enemies!");
            infoPanel.gameObject.SetActive(true);
            return;
        }
        else if (!haveMyTeam)
        {
            info.SetText("You Need Red Castle!");
            infoPanel.gameObject.SetActive(true);
            return;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("game3D");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("play scene");
    }


    /*
     	public GameObject settingPanel;


	void Start () 
	{
		if(settingPanel != null)
		{
			settingPanel.SetActive(false);
		}
	}
    public void OnBtnSetting()
    {
        settingPanel.SetActive(true);
    }
     */
}

