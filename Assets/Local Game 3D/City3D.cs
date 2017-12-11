using UnityEngine;
using System.Collections;
using System;

public class City3D : City
{
    public TextMesh popText;
    public LineRenderer lineRender;
    public BoxCollider boxCollider;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    public GameObject flag;
    public SkinnedMeshRenderer flagMeshRenender;
    public bool isDummy = false;

    void Start()
    {
        lineRender.SetPosition(0, transform.position + Vector3.up);
        OnGameStart();
        HideFlag();
    }

    public override void SetUp(int x, int y, int typeID, Team team)
    {
        base.SetUp(x, y, typeID, team);
        this.meshFilter.mesh = Data.inst.cityInfos[typeID].mesh;
        this.transform.position = GameManager3D.XYtoVector3(x, y);
    }

    void OnMouseExit()
    {
        transform.localScale = Vector3.one;
        GameManager3D.inst.HideTargetObj();
        if (GameManager3D.inst.choosing)//如果在选择情况下
        {
            transform.localScale *= 1f / 1.1f;
            if (IsSameTeam(GameManager3D.inst.playerTeam))//如果是自己队伍
            {
                if (!GameManager3D.inst.MyFromCities.Contains(this))
                {//如果不是输出国

                    GameManager3D.inst.MyFromCities.Add(this);//成为输出国
                    lineRender.enabled = true;
                    StartCoroutine(drawLine());
                }
            }
            GameManager3D.inst.MyTargetCity = null;//无论是否为盟军，皆不为输入国
        }
    }

    void OnMouseEnter()
    {
        if (GameManager3D.inst.choosing)
        {
            GameManager3D.inst.ShowTargetObj(this);
            transform.localScale *= 1.1f;
            GameManager3D.inst.MyTargetCity = this;
        }
    }


    //画射线
    IEnumerator drawLine()
    {
        while (GameManager3D.inst.choosing)
        {
            /*
            //得到现在鼠标的2维坐标系位置  
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            //将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动量  
            Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);
            CurPosition.y = 0;
            //CurPosition就是物体应该的移动向量赋给transform的position属性        
            lineRender.SetPosition(1, CurPosition);
            */
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            //Physics.Raycast(ray, )
            lineRender.SetPosition(1, hit.point + Vector3.up);
            yield return null;
        }
        lineRender.enabled = false;
    }


    //人口增长
    IEnumerator grow()
    {
        while (!isDummy)
        {
            yield return new WaitUntil(() => GameManager3D.inst.isPlaying == true);
            if (GetPopulation() < GetTopPop())
            {
                SetPopulation(GetPopulation() + 1);
                yield return new WaitForSeconds(1.0f / GameManager3D.inst.gameSpeed - 0.15f * bCode);
                
            }
            else if (GetPopulation() == GetTopPop())
            {
                yield return new WaitUntil(() => GetPopulation() != GetTopPop());
            }
            else if (GetPopulation() > GetTopPop())
            {
                SetPopulation(GetPopulation() - 1);
                yield return new WaitForSeconds(0.1f / GameManager3D.inst.gameSpeed);
            }
        }
    }

    private void ShowFlag()
    {
        flag.transform.localPosition = Data.inst.cityInfos[bCode].flagPos;
        flagMeshRenender.material.color = Data.inst.colorList[(int)team];
        flag.gameObject.SetActive(true);
    }

    private void HideFlag()
    {
        flag.gameObject.SetActive(false);
    }

    protected override void ShowPopulation()
    {
        int pop = GetPopulation();
        popText.text = "" + pop;
        if (pop >= GetTopPop()) { ShowFlag(); }
        else { HideFlag(); }
    }

    protected override void ShowTeam()
    {
        meshRenderer.materials[1].color = Data.inst.colorList[(int)team];
    }

    public override void StartGrowing()
    {
        StartCoroutine(grow());
    }




}
