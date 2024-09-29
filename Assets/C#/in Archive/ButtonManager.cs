using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    // ボタンに必要なデータを保持するクラス
    [System.Serializable]
    public class ButtonData
    {
        public string buttonText;       // ボタンのテキスト
        public Sprite buttonImage;      // ボタンに表示する画像
        public int state;               // 変数1: 0=???表示, 1=通常表示
        public string description1;     // 説明1
        public string description2;     // 説明2
        public Sprite descriptionImage; // 説明画面の中央に表示する画像
    }

    // 複数のボタンデータを格納するリスト
    public List<ButtonData> buttonDataList = new List<ButtonData>();
    
    public GameObject buttonPrefab;       // ボタンのプレハブ
    public Transform contentParent;       // ボタンを配置するScrollViewのContent

    // 説明画面のUI要素
    public GameObject descriptionPanel;   // 説明画面全体
    public Image descriptionImage;        // 説明画面の中央の画像
    public TextMeshProUGUI descriptionText1; // 説明画面の説明1
    public TextMeshProUGUI descriptionText2; // 説明画面の説明2
    public Button backButton;             // 戻るボタン

    private void Start()
    {
        // 複数のボタンを生成
        for (int i = 0; i < buttonDataList.Count; i++)
        {
            CreateButton(i);
        }

        // 説明パネルと戻るボタンの初期設定
        descriptionPanel.SetActive(false);
        backButton.onClick.AddListener(HideDescriptionPanel);
    }

    // ボタンを生成して設定
    void CreateButton(int index)
    {
        ButtonData data = buttonDataList[index];
        
        // ボタンを生成
        GameObject newButton = Instantiate(buttonPrefab, contentParent);
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        Image buttonImage = newButton.transform.Find("ButtonImage").GetComponent<Image>();

        // 状態に応じたテキストと画像設定
        if (data.state == 0)
        {
            buttonText.text = "???";
            buttonImage.gameObject.SetActive(false); // 画像を非表示
        }
        else if (data.state == 1)
        {
            buttonText.text = data.buttonText;
            buttonImage.sprite = data.buttonImage;
            buttonImage.gameObject.SetActive(true); // 画像を表示
        }

        // ボタンがクリックされたときの動作
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => OnButtonClicked(data));
    }

    // ボタンがクリックされたときに説明画面を表示
    void OnButtonClicked(ButtonData data)
    {
        if (data.state == 1)
        {
            ShowDescriptionPanel(data);
        }
    }

    // 説明画面を表示
    void ShowDescriptionPanel(ButtonData data)
    {
        descriptionPanel.SetActive(true);
        descriptionImage.sprite = data.descriptionImage;
        descriptionText1.text = data.description1;
        descriptionText2.text = data.description2;
    }

    // 説明画面を隠す
    void HideDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
    }
}
