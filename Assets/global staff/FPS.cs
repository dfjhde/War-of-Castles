using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour {

    public Text fpsT;
    public float f10;
    public int count;
	// Update is called once per frame
	void Update () {
        if (count < 10)
        {
            f10 += Time.deltaTime;
            count++;
        }
        else
        {
            fpsT.text = "FPS: " + (600 / f10);
            count = 0;
            f10 = 0f;
        }
        
	}
}
