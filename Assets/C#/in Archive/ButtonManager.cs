using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons;                  // ボタンの配列
    public ButtonData[] buttonDataList;       // 各ボタンに対応するデータ配列
    public GameObject descriptionPanel;       // 説明を表示するパネル
    public Image descriptionImage;            // 説明に表示する画像
    public TextMeshProUGUI description1Text;  // 説明テキスト1
    public TextMeshProUGUI description2Text;  // 説明テキスト2
    public Button backButton;                 // 戻るボタン

    private void Start()
    {
        // ボタンにリスナーを設定
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;  // クロージャ対策のためローカル変数に保持
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));

            // ボタンのテキストと画像を設定
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = buttonDataList[i].buttonText;
            buttons[i].GetComponentInChildren<Image>().sprite = buttonDataList[i].buttonImage;
        }

        // 戻るボタンのクリックイベント
        backButton.onClick.AddListener(HideDescriptionPanel);
        
        descriptionPanel.SetActive(false);
    }

    // ボタンがクリックされたときの処理
    private void OnButtonClicked(int index)
    {
        ButtonData data = buttonDataList[index];

        // 説明パネルにデータをセット
        descriptionImage.sprite = data.descriptionImage;
        description1Text.text = data.description1;
        description2Text.text = data.description2;

        // 説明パネルを表示
        descriptionPanel.SetActive(true);
    }

    // 説明パネルを隠す処理
    private void HideDescriptionPanel()
    {
        descriptionPanel.SetActive(false);
    }
}
