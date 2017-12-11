using UnityEngine;
using System.Collections;
using System;

public class Army3D : Army
{

    public MaterialIndex[] materialIndexs;
    public TextMesh popText;

    public Vector2 startPos;
    public Vector2 endPos;

    // Use this for initialization
    public override void Setup(int population, Team team, City from, City to)
    {
        base.Setup(population, team, from, to);
        startPos = fromCity.GetPostion();
        endPos = toCity.GetPostion();

        transform.LookAt(toCity.transform);
        //popText.transform.LookAt(Camera.main.transform);
        popText.transform.rotation = Camera.main.transform.rotation;
    }

    protected override void UpdatePostion()
    {
        go += Time.deltaTime * GameManager.ARMY_SPEED * GameManager.inst.gameSpeed;
        Vector2 pos = Vector2.MoveTowards(startPos,endPos, go);
        transform.position = GameManager3D.XYtoVector3((int)pos.x, (int)pos.y);
    }

    protected override void ShowPopulation()
    {
        popText.text = "" + GetPopulation();
    }

    protected override void ShowTeam()
    {
        foreach (MaterialIndex mi in materialIndexs)
        {
            mi.setColor(Data.inst.colorList[(int)team]);
        }
        /*
        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.materials[0].color = Data.inst.colorList[(int)team];
        }
        */
    }

    public override Vector2 GetPostion()
    {
        return GameManager3D.Vector3ToXY(transform.position);
    }
}
