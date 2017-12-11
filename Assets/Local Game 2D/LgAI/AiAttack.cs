using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum pathMethod { closeAllcity, closelastCity, closelastCityAndBaseCity }

public class AiAttack : AiBase
{
    public float look_per_city_time;

    public City firstCity;
    public List<City> attackPathCities;

    List<City> myCities = new List<City>();
    List<City> enemyCities = new List<City>();

    public bool use_train_moving;
    public bool use_move_full_city_to_next_one;
    public bool use_one_move_per_sec;
    public bool use_Fast_on_nentual;
    public bool use_Best_one_Go;
    public bool use_Fast_on_nentual_Best_one_Go;
    public bool isShowPath;

    public pathMethod path_method;

    public int popNeeded;

    public override void OnGameStart()  
	{
        ReflashList();
        if (myCities.Count == 0)
        {
            Destroy(this.gameObject);
            return;
        }
        findPath();
        if (use_train_moving)               { StartCoroutine(train_moving()); }
        if (use_move_full_city_to_next_one) { StartCoroutine(move_full_city_to_next_one()); }
        if (use_one_move_per_sec)           { StartCoroutine(one_move_per_sec()); }
        if (use_Fast_on_nentual)            { StartCoroutine(Fast_on_nentual()); }
        if (use_Best_one_Go)                { StartCoroutine(Best_one_Go()); }
        if (use_Fast_on_nentual_Best_one_Go){ StartCoroutine(Fast_on_nentual_Best_one_Go()); }

    }

    //find all my cities and enemy's cities
    private void ReflashList()
    {
        myCities.Clear();
        enemyCities.Clear();

        foreach (City city in GameManager.inst.allCities)
        {
            if (isMine(city))
            {
                myCities.Add(city);
            }
            else
            {
                enemyCities.Add(city);
            }
        }


    }

    //find a path connect all the cities in the map
    private void findPath()
    {
        Debug.Log(this.gameObject.name);
        //get the city to start with
        if (firstCity == null)
        {
            firstCity = myCities[0];
        }
        
        attackPathCities = new List<City>();

        changeList(firstCity, myCities, attackPathCities);

        switch (path_method)
        {
            case pathMethod.closeAllcity:
                pathcloseAllcity();
                break;

            case pathMethod.closelastCity:
                pathcloselastCity();
                break;

            case pathMethod.closelastCityAndBaseCity:
                pathcloselastCityAndBaseCity();
                break;

            default:
                break;
        }

        if (isShowPath) { ShowPath(); }
        
    }


    //path finding method
    private void pathcloseAllcity()
    {
        //find path from all my cities
        while (myCities.Count > 0)
        {
            City city = closestCity(attackPathCities, myCities);
            changeList(city, myCities, attackPathCities);
        }

        //find path from all my enemy cities
        while (enemyCities.Count > 0)
        {
            City city = closestCity(attackPathCities, enemyCities);
            changeList(city, enemyCities, attackPathCities);
        }
    }

    private void pathcloselastCity()
    {
        //find path from all my cities
        while (myCities.Count > 0)
        {
            City city = closestCity(attackPathCities[attackPathCities.Count - 1], myCities);
            changeList(city, myCities, attackPathCities);
        }

        //find path from all my enemy cities
        while (enemyCities.Count > 0)
        {
            City city = closestCity(attackPathCities[attackPathCities.Count - 1], enemyCities);
            changeList(city, enemyCities, attackPathCities);
        }
    }

    private void pathcloselastCityAndBaseCity()
    {
        //find path from all my cities
        while (myCities.Count > 0)
        {
            List<City> cities = new List<City>();
            cities.Add(attackPathCities[attackPathCities.Count - 1]);
            cities.Add(firstCity);
            City city = closestCity(cities, myCities);

            changeList(city, myCities, attackPathCities);
        }

        //find path from all my enemy cities
        while (enemyCities.Count > 0)
        {
            List<City> cities = new List<City>();
            cities.Add(attackPathCities[attackPathCities.Count - 1]);
            cities.Add(firstCity);
            City city = closestCity(cities, enemyCities);
            changeList(city, enemyCities, attackPathCities);
        }
    }




    //moving method
    IEnumerator train_moving()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager.inst.gameSpeed);

            for (int i = 0; i < attackPathCities.Count-1; i++)
            {
                if (isMine(attackPathCities[i]))
                {
                    if (attackPathCities[i].hasPopulation(0.7f))
                    {
                        GameManager.inst.BuildFroce(attackPathCities[i], attackPathCities[i + 1]);
                        yield return null;
                    }
                }
                else { break; }
            }
        }
    }

    IEnumerator move_full_city_to_next_one()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager.inst.gameSpeed);

            for (int i = 0; i < attackPathCities.Count - 1; i++)
            {
                if (isMine(attackPathCities[i]))
                {
                    if (attackPathCities[i].hasPopulation(1f))
                    {
                        GameManager.inst.BuildFroce(attackPathCities[i], attackPathCities[i + 1]);
                        yield return null;
                    }
                }
                else { break; }
            }
        }
    }

    IEnumerator one_move_per_sec()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager.inst.gameSpeed);

            City targetCity = attackPathCities.Find(nextTarget);
            City bestCity = attackPathCities.Find(my100City);

            GameManager.inst.BuildFroce(bestCity, targetCity);
        }
    }

    IEnumerator Fast_on_nentual()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager.inst.gameSpeed);

            City targetCity = attackPathCities.Find(nextTarget);
            City bestCity;
            if (targetCity.IsNeutral())
            {
                popNeeded = targetCity.GetPopulation()*2 + 1;
                bestCity = attackPathCities.Find(goodEnoughCity);
                if (bestCity == null)
                {
                    bestCity = attackPathCities.Find(my100City);
                }
            }
            else
            {
                bestCity = attackPathCities.Find(my100City);
            }
            GameManager.inst.BuildFroce(bestCity, targetCity);
        }
    }

    IEnumerator Best_one_Go()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager.inst.gameSpeed);

            City targetCity = attackPathCities.Find(nextTarget);
            List<City> bestCities = attackPathCities.FindAll(my75City);
            
            if ( bestCities.Count > 0)
            {
                bestCities.Sort(myBestCity);
                GameManager.inst.BuildFroce(bestCities[bestCities.Count-1], targetCity);
            }
            
        }
    }

    IEnumerator Fast_on_nentual_Best_one_Go()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.inst.isPlaying == true);

            yield return new WaitForSeconds(timeInterval / GameManager.inst.gameSpeed + look_per_city_time * attackPathCities.FindAll(myCity).Count);

            City targetCity = attackPathCities.Find(nextTarget);
            
            City bestCity = null;
            if (targetCity != null)
            {
                if (targetCity.IsNeutral())
                {
                    bestCity = GetBestCityOver(my50City);
                }
                else
                {
                    bestCity = GetBestCityOver(my100City);
                }

                if (bestCity != null) { GameManager.inst.BuildFroce(bestCity, targetCity); }
                //else { Debug.Log("no best city"); }
            }

          
        }
    }


    private City GetBestCityOver(Predicate<City> match)
    {
        List<City> myReadyCities = attackPathCities.FindAll(match);
        if (myReadyCities.Count > 0)
        { return GetBestCityFrom(myReadyCities); }
        else { return null; }
    }

    private City GetBestCityFrom(List<City> fromCities)
    {
        List<City> cities = new List<City>(fromCities);
        cities.Sort(myBestCity);
        return cities[cities.Count - 1];
    }


    //c# comparetor and predect
    private bool myCity(City city)
    {
        return isMine(city);
    }

    private int myBestCity(City x, City y)
    {
        return x.GetPopulation().CompareTo(y.GetPopulation());
    }

    private bool nextTarget(City city)
    {
        return !isMine(city);
    }

    private bool my100City(City city)
    {
        return city.hasPopulation(1f) && isMine(city);
    }

    private bool my75City(City city)
    {
        return city.hasPopulation(0.75f) && isMine(city);
    }

    private bool my50City(City city)
    {
        return city.hasPopulation(0.5f) && isMine(city);
    }

    private bool my35City(City city)
    {
        return city.hasPopulation(0.35f) && isMine(city);
    }

    private bool goodEnoughCity(City city)
    {
        return city.hasPopulation(popNeeded) && isMine(city);
    }



    public LineRenderer line;
    
    public void ShowPath()
    {
        line.SetVertexCount(attackPathCities.Count);
        line.SetWidth(0.1f, 1f);
        for(int i = 0; i < attackPathCities.Count; i++)
        {
            line.SetPosition(i, attackPathCities[i].transform.position);
        }
        
    }
}
