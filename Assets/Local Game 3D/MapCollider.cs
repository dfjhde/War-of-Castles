using UnityEngine;
using System.Collections;

public class MapCollider : MonoBehaviour
{
    MapCollider inst;
    public bool IsOnMap;

	// Use this for initialization
	void Start ()
    {
        inst = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnMouseEnter()
    {
        IsOnMap = true;
    }

    private void OnMouseExit()
    {
        IsOnMap = false;
    }
}
