using UnityEngine;
using System.Collections;
using System;

public class Army2D : Army 
{
	
	public Vector2 startPos;
	public Vector2 endPos;

	public TextMesh popText;

    public SpriteRenderer topSr;
    public SpriteRenderer backSr;

    void Start () 
	{
		startPos=new Vector2(transform.position.x, transform.position.y);
		endPos=new Vector2(toCity.transform.position.x, toCity.transform.position.y);
		topSr.flipX=startPos.x>endPos.x;
		backSr.flipX=startPos.x>endPos.x;
	}


    protected override void UpdatePostion()
    {
        go += Time.deltaTime * GameManager.ARMY_SPEED * GameManager.inst.gameSpeed;
        Vector2 pos = Vector2.MoveTowards(fromCity.GetPostion(), toCity.GetPostion(), go);
        transform.position = GameManager2D.XYtoVector2((int)pos.x, (int)pos.y);
        /*
        distance=Vector3.Distance(transform.position, toCity.transform.position);
        if(distance>0.2f)
        {
            go+=Time.deltaTime*GameManager2D.ARMY_SPEED * GameManager2D.inst.gameSpeed;
            transform.position =Vector2.MoveTowards(startPos,endPos,go);
        }
        */
    }

    protected override void ShowPopulation()
    {
        popText.text = "" + GetPopulation();
    }
    protected override void ShowTeam()
    {
        topSr.color = Data.inst.colorList[(int)team];
    }

    public override Vector2 GetPostion()
    {
        return GameManager2D.Vector2ToXY(transform.position);
    }
}


public abstract class Army : MonoBehaviour
{
    private int population;
    public Team team;
    [SerializeField]
    protected City fromCity;
    [SerializeField]
    protected City toCity;

    public float go = 0;
    public float distance = 2;


    public virtual void Setup(int population, Team team, City from, City to)
    {
        SetPopulation(population);
        Debug.Log("a0");
        SetTeam(team);
        Debug.Log("a1");
        this.fromCity = from;
        this.toCity = to;
    }

    public void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
        if (!GameManager2D.inst.isPlaying)
        {
            return;
        }

        if (!IsArrived())
        {
            UpdatePostion();
        }
        else
        {
            toCity.OnArmyArrived(GetPopulation(), GetTeam());
            GameManager2D.inst.allArmies.Remove(this);
            DestroyArmy();
        }
    }

    protected virtual bool IsArrived()
    {
        distance = toCity.DistanceTo(this);
        return distance <= 0.2f;
    }

    protected virtual void DestroyArmy()
    {
        Destroy(gameObject);
    }

    protected abstract void UpdatePostion();

    //get, set, show
    public int GetPopulation()
    {
        return this.population;
    }
    protected void SetPopulation(int population)
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

    protected void SetTeam(Team team)
    {
        this.team = team;
        Debug.Log("a0.5");
        ShowTeam();
    }
    protected abstract void ShowTeam();
    public abstract Vector2 GetPostion();
}
