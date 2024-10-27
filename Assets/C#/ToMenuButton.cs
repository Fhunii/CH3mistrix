
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ToMenuButton : MonoBehaviour
{

    void Start()
    {

    }

    public void OnClick()
    {
        StartCoroutine(Clicked());

    }
    // ボタンが押された場合、今回呼び出される関数
    public IEnumerator Clicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu scene");
        yield return null;

    }
}