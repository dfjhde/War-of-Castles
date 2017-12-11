using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BenAI : MonoBehaviour
{
    List<City> line;
    List<City> allTowerInMap;
    List<City> myTeamTower;
    List<City> enemyTower;
    List<City> whiteTower;
    void Start ()
    {
        FindLine();

        if (getHurt())
        {
            backUp();
        }
        else if (haveGoodDeal())
        {
            TakeGoodDeal();
        }
        else
        {
            Attack(GetNextTarget());
        }
	}

    private City GetNextTarget()
    {
        throw new NotImplementedException();
    }

    private void TakeGoodDeal()
    {
        throw new NotImplementedException();
    }

    private bool haveGoodDeal()
    {
        throw new NotImplementedException();
    }

    private void backUp()
    {
        throw new NotImplementedException();
    }

    private bool getHurt()
    {
        throw new NotImplementedException();
    }

    private void FindLine()
    {
        line = new List<City>();
        myTeamTower = new List<City>();
        enemyTower = new List<City>();
        whiteTower = new List<City>();
        foreach (City castle in allTowerInMap)
        {
            if (isMyTeam(castle))
            {

            }
        }
    }

    private bool isMyTeam(City castle)
    {
        throw new NotImplementedException();
    }

    void Update ()
    {
	
	}




    private void Attack(City target)
    {
        City closest =  getCloseTowerFromTarget(target);

        if (isEnoughSoldiertoAttack(target))
        {
            AllSoldierToTower(closest);
            attackFrom(closest, target);
        }
    }

    private void attackFrom(City closest, City target)
    {
        throw new NotImplementedException();
    }

    private bool isEnoughSoldiertoAttack(City target)
    {
        throw new NotImplementedException();
    }

    private void AllSoldierToTower(City closest)
    {
        throw new NotImplementedException();
    }

    private City getCloseTowerFromTarget(City target)
    {
        throw new NotImplementedException();
    }
}
