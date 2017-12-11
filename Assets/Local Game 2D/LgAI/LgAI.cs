using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LgAI : AiBase 
{
	public City baseCity;
    public City myWeakestCity;
    public City enemyWeakestCity;

    List<City> readyToGoCities = new List<City>();
    List<City> mustGoCities = new List<City>();
    List<City> mustSaveCities = new List<City>();

    List<City> myTeamCities = new List<City>();
    List<City> enemyCities = new List<City>();
    List<City> whiteCities = new List<City>();

    //-------------------------------------------------------------------------------------------------------
    //                               level 1


    public override void OnGameStart()
    {
        //FirstLookAtTheMap();

		fromCities = new List<City>();

		StartCoroutine(PlayGame());
	}



    IEnumerator PlayGame()
	{
		while(true)
		{
            yield return new WaitUntil(() => GameManager2D.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager2D.inst.gameSpeed);

            See();
            
            Think();

            Act();
		}
	}



    //                               level 1
    //-------------------------------------------------------------------------------------------------------
    //                               level 2


    private void FirstLookAtTheMap()
    {
        ReflashList();

        findPath();
    }

    private void See()
    {
        ReflashList();

        MakeSureHaveBase();
    }


    private void Think()
    {
        if (isNeedSave())
        {
            Save();
            return;
        }
        else if (isNeedAttack())
        {
            Attack();
            return;
        }
    }


    private void Act()
    {
        if (fromCities != null && targetCity != null)
        {
            GameManager2D.inst.BuildFroce(fromCities, targetCity);
        }
        fromCities.Clear();
        targetCity = null;
    }


    //                               level 2
    //-------------------------------------------------------------------------------------------------------
    //                               level 3


    private void ReflashList()
    {
        myTeamCities.Clear();
        enemyCities.Clear();
        whiteCities.Clear();

        myWeakestCity = null;
        enemyWeakestCity = null;

        foreach (City city in GameManager2D.inst.allCities)
        {
            if (isMine(city))
            {
                myTeamCities.Add(city);
                if (!myWeakestCity || myWeakestCity.GetPopulation() > city.GetPopulation())
                {
                    myWeakestCity = city;
                }
            }
            else if (isNeutral(city))
            {
                whiteCities.Add(city);
            }
            else
            {
                enemyCities.Add(city);
                if (!enemyWeakestCity || enemyWeakestCity.GetPopulation() > city.GetPopulation())
                {
                    enemyWeakestCity = city;
                }
            }
        }
    }
    

    public void MakeSureHaveBase()
    {
        if (baseCity == null)
        {
            baseCity = myTeamCities[0];
        }
    }


	bool isNeedSave()
	{
        if (!myWeakestCity.hasPopulation(0.25f))
        {
            targetCity = myWeakestCity;
            return true;
        }
        else
        {
            targetCity = null;
            return false;
        }
	}

	void Save()
	{
		foreach(City city in myTeamCities)
		{
			if(city.hasPopulation(0.5f))
			{
				if(((city.GetPopulation() / 2) + targetCity.GetPopulation() + PopIncrease(city,targetCity)) > targetCity.GetTopPop()/2 )
				{
					fromCities.Add(city);
					return;
				}
			}
		}
	}

    bool isNeedAttack()
	{
		float distance = 10000f;
		foreach(City castle in enemyCities)
		{
			float newDis= baseCity.DistanceTo(castle);
			if(newDis<distance)
			{
				distance=newDis;
				targetCity=castle;
			}
		}
		return targetCity != null;
	}

	void Attack()
	{
		foreach(City city in myTeamCities)
		{
            if (city.hasPopulation(0.5f))
			{
				fromCities.Add(city);
			}
		}
	}
    /*    */

    //---------tools------------

    private void findPath()
    {
        List<City> myTeamListTem = myTeamCities;
        List<City> enemyListTem = enemyCities;

        City firstTarget = closestCity(myTeamListTem, enemyListTem);
        
        while (enemyListTem.Count > 0)
        {

        }
    }

    /*
void Think1()
{
    Debug.Log("think 1");
    //刷新城堡列表
    ownedCastle.Clear();


    foreach(LgCastle lgc in AllCastle)
    {
        switch(lgc.sr.color == AiTeamColor)
        {
        case AiTeamColor:
            CastleAi(lgc);
            break;

        case Color.white:
            CastleNeutral(lgc);
            break;

        default:
            CastleNeutral(lgc);
            break;
        }
    }
    AiFromCastle = ReadyToGoCastle;
}

void CastleAi(LgCastle lgc)
{
    ownedCastle.Add(lgc);


    if(lgc.population < (lgc.topPop/4))
    {
        AiTargetCastle=lgc;
    }
    else if(lgc.population == lgc.topPop)
    {
        MustGoCastle.Add(lgc);
    }
    else if(lgc.population > (lgc.topPop/2))
    {
        MustSaveCastle.Add(lgc);
    }
}

void CastleNeutral(LgCastle lgc)
{
    float distance = 100f;
    float newDis=Vector3.Distance(MainCastle.transform.position,lgc.transform.position);
    if(newDis<distance)
    {
        distance=newDis;
        AiTargetCastle=lgc;
    }
}
*/


}
