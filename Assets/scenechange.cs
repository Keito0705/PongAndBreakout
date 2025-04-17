using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenechange : MonoBehaviour
{
    public void FromToPlay()
    {
        SceneManager.LoadScene("game");
    }

    public void FromToTitle()
    {
        SceneManager.LoadScene("title");
    }

}
