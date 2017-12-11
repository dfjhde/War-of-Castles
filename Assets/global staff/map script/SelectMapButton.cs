using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SelectMapButton : MonoBehaviour
{
	public RectTransform myRT;
	public Text nameT;
	public string mapCode;


	void Start(){}

	public void OnBtn()
	{

		PlayerPrefs.SetString("map",mapCode);
	}
}