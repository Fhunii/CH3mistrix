using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;  // TextMeshProを使用するためのライブラリ

public class ArchiveContentsManager : MonoBehaviour
{
    public GameObject buttonPrefab;  // ボタンのプレハブ
    public Transform scrollViewContent;  // スクロールビューのコンテント
    public GameObject square;  // スクエア
    public Button backButton;  // 戻るボタン
    public TextMeshProUGUI description1;  // 説明1 (TextMeshProに変更)
    public Image image;  // 画像
    public TextMeshProUGUI description2;  // 説明2 (TextMeshProに変更)

    private List<string> buttonTexts = new List<string> { "ボタン1", "ボタン2", "ボタン3" };  // ボタンのテキスト
    private List<string> descriptions1 = new List<string> { "説明1-1", "説明1-2", "説明1-3" };  // 説明1
    private List<Sprite> images = new List<Sprite>();  // 画像リスト
    private List<string> descriptions2 = new List<string> { "説明2-1", "説明2-2", "説明2-3" };  // 説明2

    void Start()
    {
        InitializeButtons();
        HideSquare();  // 初期状態でスクエアを非表示

        backButton.onClick.AddListener(HideSquare);  // 戻るボタンにイベントを追加
    }

    // ボタンを初期化する
    void InitializeButtons()
    {
        for (int i = 0; i < buttonTexts.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, scrollViewContent);
            int index = i;  // ローカル変数として保持
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonTexts[i];  // TextMeshProUGUIに対応
            newButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));
        }
    }

    // ボタンが押されたときの処理
    void OnButtonClick(int index)
    {
        ShowSquare(index);
        ToggleButtons(false);  // 他のボタンを無効化
    }

    // スクエアとその要素を表示する
    void ShowSquare(int index)
    {
        square.SetActive(true);
        description1.text = descriptions1[index];
        image.sprite = images[index];
        description2.text = descriptions2[index];
        backButton.gameObject.SetActive(true);
    }

    // スクエアとその要素を隠す
    void HideSquare()
    {
        square.SetActive(false);
        backButton.gameObject.SetActive(false);
        ToggleButtons(true);  // 他のボタンを再度有効化
    }

    // ボタンの有効化・無効化を切り替える
    void ToggleButtons(bool isEnabled)
    {
        foreach (Transform child in scrollViewContent)
        {
            child.GetComponent<Button>().interactable = isEnabled;
        }
    }
}
