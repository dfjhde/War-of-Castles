using UnityEngine;
using System.Collections;

public class OnlineScene : MonoBehaviour
{
    public void OnBtnMakeMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("makeMap");
    }

    public void OnBtnChallenge()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("challenge");
    }

    public void OnBtnBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }
}
