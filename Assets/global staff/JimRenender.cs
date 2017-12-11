using UnityEngine;
using System.Collections;

public class JimRenender : MonoBehaviour
{

}

[System.Serializable]
public class MaterialIndex
{
    public MeshRenderer meshRenender;
    public int index;

    public void setColor(Color color)
    {
        meshRenender.materials[index].color = color;
    }
}
