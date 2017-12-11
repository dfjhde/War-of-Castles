using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class OverManager : MonoBehaviour
{
    public Button btnNextLevel;
    public Button btnAgain;
    public Button btnUpload;
    public Button btnBack;
    public Button btnBackViaStop;

    public InputField authorNameIF;
    public Button btnStop;

    public GameObject overPanelGo;
    public Text overText;

    public Text uploadText;

    void Start ()
    {
        btnNextLevel.gameObject.SetActive(false);
        btnUpload.gameObject.SetActive(false);
        overPanelGo.SetActive(false);
        //set up back button
        btnBack.onClick.RemoveAllListeners();
        btnBackViaStop.onClick.RemoveAllListeners();
        switch (Data.inst.gameMode)
        {
            case GameMode.Campaign:
                btnBack.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene("Campaign"); });
                btnBackViaStop.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene("Campaign"); });
                break;

            case GameMode.Challenge:
                btnBack.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge"); });
                btnBackViaStop.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge"); });
                break;

            case GameMode.MakeMap:
                btnBack.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene("makeMap"); });
                btnBackViaStop.onClick.AddListener(delegate { UnityEngine.SceneManagement.SceneManager.LoadScene("makeMap"); });
                break;

            case GameMode.PvP:
                btnBack.onClick.AddListener(delegate { PvpGameManager.instPvp.OnBtnBackToLobby(); });
                break;

            default:
                break;
        }
    }

    public void OnOver(bool isWin)
    {

        //setup the buttons
        if (isWin)
        {
            switch (Data.inst.gameMode)
            {
                case GameMode.Campaign:
                    btnNextLevel.gameObject.SetActive(true);
                    break;

                case GameMode.Challenge:
                    break;

                case GameMode.MakeMap:
                    btnUpload.gameObject.SetActive(true);
                    break;

                case GameMode.Testing:
                    break;

                case GameMode.PvP:
                    //PvpGameManager.instPvp.localPvpPlayer.callCmdWin();
                    break;
                default:
                    break;
            }
        }
        overText.text = isWin ? "You Win!" : "You Lose!";
        overPanelGo.SetActive(true);
        btnStop.gameObject.SetActive(false);

        if (Data.inst.gameMode != GameMode.PvP)
        {
            btnAgain.gameObject.SetActive(!isWin);
        }


    }

    public void OnBtnNextLevel()
    {
        int nextMapId = Data.inst.GetCurrentMapInfo().id + 1;
        Data.inst.SetCurrentMap(nextMapId, GameMode.Campaign);
        UnityEngine.SceneManagement.SceneManager.LoadScene("game3D");
        //UnityEngine.SceneManagement.SceneManager.LoadScene("play scene");
    }

    public void OnBtnUpload()
    {
        StartCoroutine(Upload());
    }

    private IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("userName", authorNameIF.text);
        form.AddField("name", "playerMade");
        form.AddField("code", Data.inst.GetCurrentMapInfo().GetMapCode());
        Debug.Log(Data.inst.GetCurrentMapInfo().GetMapCode());

        UnityWebRequest www = UnityWebRequest.Post("http://madebyjimchen.com/WarOfCastles/api/uploadMap.php", form);
        yield return www.Send();
        btnUpload.interactable = false;
        uploadText.text = "Uploading...";
        
        if (www.isNetworkError)
        {
            uploadText.text = "Network Error";
        }
        else if (www.isHttpError)
        {
            uploadText.text = "Server Error";
        }
        else
        {
            overText.text = "complete!";
            PlayerPrefs.SetString("challengePanel", "myMap");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Challenge");
        }
        Debug.Log(www.downloadHandler.text);

    }
}

