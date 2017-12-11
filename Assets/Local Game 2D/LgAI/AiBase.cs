using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AiBase : MonoBehaviour
{
    public float timeInterval = 1f;
    public Team myTeam;
    public City targetCity;
    public List<City> fromCities;

    public void Awake()
    {
        //GameManager2D.inst.AIs.Add(this);
    }

    public abstract void OnGameStart();

    protected bool isMine(City city)
    {
        return city.IsSameTeam(myTeam);
    }

    protected bool isNeutral(City city)
    {
        return city.IsSameTeam(Data.inst.GetNeutralTeam());
    }


    // find the city that is closest to the fromList from targetList
    protected static City closestCity(List<City> fromCities, List<City> searchCities)
    {
        float closestDistance = 10000f;
        City closestCastle = null;

        foreach (City city in searchCities)
        {
            float averageDis = averageDistance(fromCities, city);
            if (closestDistance > averageDis)
            {
                closestDistance = averageDis;
                closestCastle = city;
            }
        }
        return closestCastle;
    }

    // find the city that is closest to the fromCity from searchCities
    protected static City closestCity(City FromCity, List<City> searchCities)
    {
        float closestDistance = 10000f;
        City closestCity = null;

        foreach (City city in searchCities)
        {
            float distance = FromCity.DistanceTo(city);
            if (closestDistance > distance)
            {
                closestDistance = distance;
                closestCity = city;
            }
        }
        return closestCity;
    }

    //the average distance from a list of castle to target castle
    protected static float averageDistance(List<City> fromCities, City target)
    {
        float distance = 0;
        foreach (City city in fromCities)
        {
            distance += city.DistanceTo(target);
        }
        distance = distance / fromCities.Count;
        return distance;
    }

    //find the amount of population increase by the time army arrived
    protected static int PopIncrease(City from, City target)
    {
        float distance = Vector3.Distance(from.transform.position, target.transform.position);
        return (int)(distance / GameManager2D.ARMY_SPEED);
    }



    //move a city from one list to another list
    protected void changeList(City city, List<City> fromCities, List<City> toCities)
    {
        fromCities.Remove(city);
        toCities.Add(city);
    }

    //find the city that have most population
    protected City GetBiggestCity(List<City> cities)
    {
        int biggest = 0;
        City biggestCity = null;

        foreach (City city in cities)
        {
            if (city.GetPopulation() > biggest)
            {
                biggest = city.GetPopulation();
                biggestCity = city;
            }
        }
        return biggestCity;
    }

}
