using UnityEngine;
using System;
using System.Collections;

public class City2D : City 
{
	public TextMesh popText;
	public SpriteRenderer topSr;
	public SpriteRenderer backSr;
	public LineRenderer lineRender;
	public BoxCollider2D bc2d;

	void Start () 
	{
		lineRender.SetPosition(0,transform.position+Vector3.back);
        OnGameStart();
	}

    public override void SetUp(int x, int y, int typeID, Team team)
    {
        base.SetUp(x, y, typeID, team);
        this.topSr.sprite = Data.inst.topList[typeID];
        this.backSr.sprite = Data.inst.backList[typeID];
        this.bc2d.offset = Data.inst.cityInfos[typeID].bc2dOffset;
        this.bc2d.size = Data.inst.cityInfos[typeID].bc2dSize;
    }

	void OnMouseExit () 
	{
		transform.localScale = Vector3.one * Data.inst.scale; ;
		if(GameManager2D.inst.choosing)//如果在选择情况下
		{
			transform.localScale *= 1f/1.1f;
			if(IsSameTeam( GameManager2D.inst.playerTeam))//如果是自己队伍
			{
				if(!GameManager2D.inst.MyFromCities.Contains(this))
                {//如果不是输出国

					GameManager2D.inst.MyFromCities.Add(this);//成为输出国
					lineRender.enabled=true;
					StartCoroutine(drawLine());
				}
			}
			GameManager2D.inst.MyTargetCity=null;//无论是否为盟军，皆不为输入国
		}
	}

	void OnMouseEnter () 
	{
		if(GameManager2D.inst.choosing)
		{
			transform.localScale *= 1.1f;
			GameManager2D.inst.MyTargetCity=this;
		}
	}

	//画射线
	IEnumerator drawLine()
	{
		while(GameManager2D.inst.choosing)
		{
			//得到现在鼠标的2维坐标系位置  
			Vector3 curScreenSpace =  new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);     
			//将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动量  
			Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);
			CurPosition.z=0;
			//CurPosition就是物体应该的移动向量赋给transform的position属性        
			lineRender.SetPosition(1,CurPosition+Vector3.back);
			yield return null;
		}
		lineRender.enabled=false;
	}

	//人口增长
	IEnumerator grow()
	{
		while(true)
		{
            yield return new WaitUntil(() => GameManager2D.inst.isPlaying == true);

            if (GetPopulation() < GetTopPop())
			{
                SetPopulation(GetPopulation() + 1);
                yield return new WaitForSeconds(1.0f/GameManager2D.inst.gameSpeed - 0.15f*bCode);
			}
			else if(GetPopulation() == GetTopPop())
			{
				yield return new WaitUntil(()=> GetPopulation() != GetTopPop());
			}
			else if(GetPopulation() > GetTopPop())
			{
				SetPopulation(GetPopulation() - 1);//(int)Mathf.Lerp((float)population,(float)topPop,0.7f);
				yield return new WaitForSeconds(0.1f/GameManager2D.inst.gameSpeed);
			}	
		}
	}




    protected override void ShowPopulation()
    {
        popText.text = "" + GetPopulation();
    }

    protected override void ShowTeam()
    {
        topSr.color = Data.inst.colorList[(int)team];
    }

    public override void StartGrowing()
    {
        StartCoroutine(grow());
    }

}


public abstract class City: MonoBehaviour
{
    private int x;
    private int y;
    private int population;
    public int bCode;//building code
    public Team team;

    public void OnGameStart()
    {
        SetPopulation(Data.inst.cityInfos[bCode].startPop);

        if (!IsNeutral())
        {
            StartGrowing();
        }
    }

    public virtual void OnArmyArrived(int armyPop, Team armyTeam)
    {
        //Debug.Log(armyTeam +" "+ this.team +" "+ IsSameTeam(armyTeam));
        //如果颜色不一样
        if (!IsSameTeam(armyTeam))
        {
            //如果守军多于攻方的话
            if (population > armyPop)
            {
                SetPopulation(CalculateSurPop(population, armyPop));
            }
            else        //如果攻方多于守军的话
            {
                SetPopulation(CalculateSurPop(armyPop, population));
                //如果还没被占领过
                if (IsNeutral())
                {
                    StartGrowing();
                }
                SetTeam(armyTeam);
            }
        }
        //如果颜色一样
        else
        {
            SetPopulation(population + armyPop);
        }
    }
    public bool IsNeutral()
    {
        return IsSameTeam(Data.inst.GetNeutralTeam());
    }

    int CalculateSurPop(int morePop, int lessPop)
    {
        //计算人口比例
        float rate = 0f;
        if (lessPop - 1 > 0)
        {
            rate = morePop / lessPop - 1;
        }


        if (rate > GameManager.TOP_RATE)//碾压胜利
        {
            return morePop;
        }
        else//有代价胜利
        {

            float surRate = rate / GameManager.TOP_RATE; //幸存比例

            float dieRate = 1 - surRate;//死亡比例

            int diePop = (int)(dieRate * lessPop);//死亡人数

            return morePop - diePop;//总幸存人数
        }
        //return 0;
    }

    public bool IsSameTeam(City otherCity)
    {
        return this.GetTeam() == otherCity.GetTeam();
    }

    public bool IsSameTeam(Team team)
    {
        return this.GetTeam() == team;
    }

    public bool hasPopulation(float radio)
    {
        return population >= GetTopPop() * radio;
    }

    public bool hasPopulation(int num)
    {
        return population >= num;
    }

    public int GetTopPop()
    {
        return Data.inst.cityInfos[bCode].topPop;
    }

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //get, set, show
    public int GetPopulation()
    {
        return this.population;
    }
    public void SetPopulation(int population)
    {
        this.population = population;
        ShowPopulation();
    }
    protected abstract void ShowPopulation();

    //get, set, show
    public Team GetTeam()
    {
        return team;
    }
    public void SetTeam(Team team)
    {
        this.team = team;
        ShowTeam();
    }
    protected abstract void ShowTeam();

    public abstract void StartGrowing();

    public Vector2 GetPostion()
    {
        return new Vector2(x, y);
    }

    public float DistanceTo(City toCity)
    {
        return Vector2.Distance(this.GetPostion(), toCity.GetPostion());
    }

    public float DistanceTo(Army army)
    {
        return Vector2.Distance(this.GetPostion(), army.GetPostion());
    }

    public virtual void SetUp(int x, int y, int typeID, Team team)
    {
        GameManager.inst.allCities.Add(this);
        SetTeam(team);
        SetXY(x, y);
        this.bCode = typeID;
    }
}


