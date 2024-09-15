using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.Compilation;


public class CompoundButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText; // ボタンのテキスト表示
    public TextMeshProUGUI compoundName; // 化合物の名前
    public UnityEngine.UI.Image buttonImage; // ボタンの画像表示
    public GameObject Panel; // ボタンの親オブジェクト
    private Action<string> onClickAction; // クリック時のアクションを外部から設定

    void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>(); // ボタンのテキストを取得
        buttonImage = GetComponent<UnityEngine.UI.Image>(); // ボタンの画像を取得
        // パネルを取得
        Panel = transform.parent.gameObject;
        //パネルを隠す
        Panel.SetActive(false);
        if (compoundName == null)
        {
            //ボタンのテキストを???に変更
            buttonText.text = "???";
            //ボタンの画像をかくす
            buttonImage.enabled = false;
        }
    }

    public void OnClick()
    {
        StartCoroutine(CompoundClicked());
    }

    public void Initialize(string name, Action<string> action)
    {
        compoundName.text = name;
        buttonText.text = name; // ボタンのテキストを設定
        onClickAction = action; // 外部からのアクションを設定
    }

    public IEnumerator CompoundClicked()
    {
        yield return new WaitForSeconds(0.1f);
        OnButtonClick();
    }
    private void OnButtonClick()
    {
        onClickAction?.Invoke(compoundName.text); // クリック時に設定されたアクションを実行
    }
}
