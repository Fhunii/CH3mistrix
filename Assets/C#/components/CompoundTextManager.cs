using UnityEngine;
using TMPro; // TextMeshProを使用するための名前空間

public class CompoundTextManager : MonoBehaviour
{
    public TextMeshProUGUI CompoundText; // TextからTextMeshProUGUIに変更


    void Start()
    {
        ResetScore();
    }

    public void ChangeCompoundText(string text)
    {
        CompoundText.text = text;
    }


    public void ResetScore()
    {
        CompoundText.text = "";
    }

    // 現在のスコアを取得するためのメソッドを追加
}
