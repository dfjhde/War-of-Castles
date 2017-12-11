using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BtnLevel : MonoBehaviour 
{
    public int mapID;
    public bool isWin;
    public string levelName;

    public Image img;
    public Text nameText;

    public void Setup(MapInfo mapInfo)
    {

        this.mapID = mapInfo.id;
        this.isWin = Data.inst.GetLevelStage(mapID) == LevelStage.Won;
        nameText.text = mapInfo.name;
        if (isWin)
        {
            img.color = Color.green;
        }
    }

	public void OnBtnLevel()
	{
        Data.inst.SetCurrentMap(mapID, GameMode.Campaign);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("play scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene("game3D");
    }
}
