using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class ArchiveContentsManager : MonoBehaviour
{    
    [System.Serializable]
    public class ButtonData
    {
        public string description1;
        public string description2;
        public string imagePath;
    }
    
    [System.Serializable]
    public class ButtonDataList
    {
        public List<ButtonData> buttonData;
    }

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject square;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI description1Text;
    [SerializeField] private Image imageDisplay;
    [SerializeField] private TextMeshProUGUI description2Text;

    private List<Button> buttons = new List<Button>();
    private ButtonDataList buttonDataList;

    void Start()
    {
        LoadDataFromJson();
        InitializeButtons();
        HideSquare();
        backButton.onClick.AddListener(HideSquare);
    }
    


    void LoadDataFromJson()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("archive data");
        buttonDataList = JsonUtility.FromJson<ButtonDataList>("{\"archive Data\":" + jsonData.text + "}");
    }

    void InitializeButtons()
    {
        for (int i = 0; i < buttonDataList.buttonData.Count; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, scrollViewContent);

            // ボタンの位置設定 (例: x=100, y=100 - i * 150)
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, 100 - i * 150);

            int index = i;  // ローカル変数として保持
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonDataList.buttonData[i].description1;
            newButton.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(index));

            buttons.Add(newButton.GetComponent<Button>());
        }
    }

    void OnButtonClick(int index)
    {
        ButtonData data = buttonDataList.buttonData[index];

        // 画像とテキストを表示する
        Sprite sprite = Resources.Load<Sprite>(data.imagePath);
        if (sprite != null)
        {
            imageDisplay.sprite = sprite;
        }
        description1Text.text = data.description1;
        description2Text.text = data.description2;

        // スクエアを最前面に表示
        square.SetActive(true);
        backButton.gameObject.SetActive(true);
        description1Text.gameObject.SetActive(true);
        imageDisplay.gameObject.SetActive(true);
        description2Text.gameObject.SetActive(true);

        // 他のボタンを無効化
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    void HideSquare()
    {
        // スクエアを非表示
        square.SetActive(false);
        backButton.gameObject.SetActive(false);
        description1Text.gameObject.SetActive(false);
        imageDisplay.gameObject.SetActive(false);
        description2Text.gameObject.SetActive(false);

        // 他のボタンを有効化
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }
}
