using UnityEngine;
using System.Collections;

public class SelectMapBtn : MonoBehaviour
{
    public TextMesh text;
    public MapInfo mapInfo;
    public SpriteRenderer backgroundSR;
    public MapSelector mapSelector;

    void Awake()
    {
        Hide();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void SetUp(MapInfo mapInfo)
    {
        text.text = "#" + mapInfo.id + " by " + mapInfo.userName;
        //text.text = "#" + mapInfo.id;
        this.backgroundSR.sprite = MapSelector.inst.mapsIMGs[mapInfo.id % MapSelector.inst.mapsIMGs.Length];
        this.mapInfo = mapInfo;
        this.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
    }

    void OnMouseDown()
    {
        mapSelector.OnBtnMap(mapInfo);
    }

}
