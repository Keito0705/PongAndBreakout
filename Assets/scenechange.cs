using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenechange : MonoBehaviour
{

    void Update()
    {
        // スペースキーが押されたらFromToPlay()を呼び出す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FromToPlay();
        }
    }

    public void FromToPlay()
    {
        SceneManager.LoadScene("game");
    }

    public void FromToTitle()
    {
        SceneManager.LoadScene("title");
    }

    public void FromToResult()
    {
        SceneManager.LoadScene("result");
    }
}
