using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;  // TextMeshProを使用するための参照

public class Piece : MonoBehaviour
{
    private CountdownManager countdownManager;
    public enum ElementType
    {
        HydrogenIon, Carbon, OxygenIon
    }

    public ElementType elementType;

    public Sprite hydrogenIonSprite;
    public Sprite carbonSprite;
    public Sprite oxygenIonSprite;

    public Sprite hydrogenIonMonoSprite;
    public Sprite carbonMonoSprite;
    public Sprite oxygenIonMonoSprite;

    private SpriteRenderer spriteRenderer;

    public GameObject sparkleEffectPrefab;
    public GameObject linePrefab;

    public GameObject compoundNameTextPrefab;  // 化合物名表示用のPrefab

    private static List<Piece> connectedPieces = new List<Piece>();
    private static Piece lastConnectedPiece;
    private static Piece firstClickedPiece;
    private static bool isDragging = false;
    private static List<LineRenderer> allLines = new List<LineRenderer>();

    private GridManager gridManager;
    private bool isActive = true;
    private static ScoreManager scoreManager;


    // 化合物ごとのスコアを設定
    private static Dictionary<List<ElementType>, (int score, string name)> compoundScores = new Dictionary<List<ElementType>, (int, string)>()
    {
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon }, (10, "メタン\n(CH<sub>4</sub>)") }, // メタン (CH₄)
        { new List<ElementType> { ElementType.Carbon, ElementType.OxygenIon }, (15, "一酸化炭素\n(CO)") }, // 一酸化炭素 (CO)
        { new List<ElementType> { ElementType.Carbon, ElementType.OxygenIon, ElementType.OxygenIon }, (20, "二酸化炭素\n(CO<sub>2</sub>)") }, // 二酸化炭素 (CO₂)
        { new List<ElementType> { ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon }, (25, "水\n(H<sub>2</sub>O)") }, // 水 (H₂O)
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon }, (30, "メタノール\n(CH<sub>3</sub>OH)") }, // メタノール (CH₃OH)
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon }, (35, "ホルムアルデヒド\n(H<sub>2</sub>CO)") }, // ホルムアルデヒド (H₂CO)
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon }, (40, "ギ酸\n(HCOOH)") }, // ギ酸 (HCOOH)
        { new List<ElementType> { ElementType.Carbon, ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon }, (45, "酢酸\n(CH<sub>3</sub>COOH)") }, // 酢酸 (CH₃COOH)
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon, ElementType.OxygenIon }, (50, "炭酸\n(H<sub>2</sub>CO<sub>3</sub>)") }, // 炭酸 (H₂CO₃)
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon, ElementType.OxygenIon }, (55, "炭酸水素イオン\n(HCO<sub>3</sub>⁻)") }, // 炭酸水素イオン (HCO₃⁻)
        // 他の組み合わせもここに追加する
    };
    
    private static Dictionary<List<ElementType>, string> compoundNames = new Dictionary<List<ElementType>, string>()
    {
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon }, "メタン (CH₄)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.OxygenIon }, "一酸化炭素 (CO)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.OxygenIon, ElementType.OxygenIon }, "二酸化炭素 (CO₂)" },
        { new List<ElementType> { ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon }, "水 (H₂O)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon }, "メタノール (CH₃OH)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon }, "ホルムアルデヒド (H₂CO)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon }, "ギ酸 (HCOOH)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon }, "酢酸 (CH₃COOH)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon, ElementType.OxygenIon }, "炭酸 (H₂CO₃)" },
        { new List<ElementType> { ElementType.Carbon, ElementType.HydrogenIon, ElementType.OxygenIon, ElementType.OxygenIon, ElementType.OxygenIon }, "炭酸水素イオン (HCO₃⁻)" }
    };

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = FindObjectOfType<GridManager>();
        scoreManager = FindObjectOfType<ScoreManager>(); // ScoreManager の参照を取得
    }

    void Start()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(AnimatePieceAppearance());
        countdownManager = FindObjectOfType<CountdownManager>();
        if (countdownManager != null && countdownManager.IsCountdownActive())
        {
            // カウントダウン中はこのスクリプトを無効にする
            this.enabled = false;
        }
    }

    private IEnumerator AnimatePieceAppearance()
    {
        Vector3 targetScale = new Vector3(0.06f, 0.06f, 0.06f);
        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public void SetElementType(ElementType type)
    {
        elementType = type;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.sprite = isActive switch
        {
            true => elementType switch
            {
                ElementType.HydrogenIon => hydrogenIonSprite,
                ElementType.Carbon => carbonSprite,
                ElementType.OxygenIon => oxygenIonSprite,
                _ => spriteRenderer.sprite
            },
            false => elementType switch
            {
                ElementType.HydrogenIon => hydrogenIonMonoSprite,
                ElementType.Carbon => carbonMonoSprite,
                ElementType.OxygenIon => oxygenIonMonoSprite,
                _ => spriteRenderer.sprite
            }
        };
    }

    public void SetActive(bool active)
    {
        isActive = active;
        UpdateSprite();
    }

    void OnMouseDown()
    {
        if (countdownManager != null && countdownManager.IsCountdownActive())
        {
            return; // カウントダウン中は処理を行わない
        }
        if (!isActive) return; // モノクロの場合は処理しない

        isDragging = true;
        connectedPieces.Clear();
        connectedPieces.Add(this);
        lastConnectedPiece = this;
        firstClickedPiece = this;

        Vector2Int gridPosition = GetGridPosition();
        if (gridManager != null)
        {
            gridManager.SetActiveArea(gridPosition);
        }
    }

    void OnMouseDrag()
    {
        if (countdownManager != null && countdownManager.IsCountdownActive())
        {
            return; // カウントダウン中は処理を行わない
        }
        if (!isDragging) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        if (hit.collider != null)
        {
            Piece hitPiece = hit.collider.GetComponent<Piece>();
            if (hitPiece != null && hitPiece.isActive && hitPiece != lastConnectedPiece)
            {
                LineRenderer blueLine = CreateLine(lastConnectedPiece.transform.position, hitPiece.transform.position);
                allLines.Add(blueLine);

                lastConnectedPiece = hitPiece;
                connectedPieces.Add(hitPiece);
                hitPiece.SetActive(false); // isActive プロパティを変更しモノクロにする

                // クリックされたスプライトもモノクロにする
                SetActive(false);
            }
        }
    }

    private LineRenderer CreateLine(Vector3 startPosition, Vector3 endPosition)
    {
        
        GameObject lineObj = Instantiate(linePrefab);
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
        return lineRenderer;
    }
    void OnMouseUp()
    {
        if (countdownManager != null && countdownManager.IsCountdownActive())
        {
            return; // カウントダウン中は処理を行わない
        }
        if (!isDragging) return;

        isDragging = false;

        bool validCompound = CheckIfValidCompound(out List<ElementType> matchedCompound);

        if (validCompound)
        {
            foreach (var piece in connectedPieces)
            {
                if (piece != null)
                {
                    piece.DestroyWithEffect();
                }
            }

            // スコアの追加
            if (matchedCompound != null && compoundScores.ContainsKey(matchedCompound))
            {
                int scoreToAdd = compoundScores[matchedCompound].score;
                scoreManager.AddScore(scoreToAdd);
                // 化合物名の表示
                string compoundName = compoundScores[matchedCompound].name;
                // ShowCompoundName(compoundName, transform.position);
                var compoundTextManager = FindObjectOfType<CompoundTextManager>();
                if (compoundTextManager != null)
                {
                    compoundTextManager.ChangeCompoundText(compoundName);
                }
            }

        }
        else
        {
            foreach (var line in allLines)
            {
                if (line != null)
                {
                    Destroy(line.gameObject);
                }
            }

            // すべてのスプライトを有効に戻す
            foreach (var piece in connectedPieces)
            {
                if (piece != null)
                {
                    piece.SetActive(true);
                }
            }
        }

        connectedPieces.Clear();
        allLines.Clear();

        gridManager.ResetActiveArea();
    }



    private void ShowCompoundName(string name, Vector3 position)
    {
        Debug.Log($"Showing compound name: {name}");  // 追加
        if (compoundNameTextPrefab == null) return;

        // Canvas の参照を取得
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene.");
            return;
        }

        // テキストオブジェクトを Canvas の子として生成
        GameObject textObj = Instantiate(compoundNameTextPrefab, canvas.transform);


        textObj.transform.position = position;

        TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();

        if (textMesh != null)
        {
            textMesh.text = name;
            StartCoroutine(AnimateTextAndDestroy(textObj, textMesh));
        }
    }

    private IEnumerator AnimateTextAndDestroy(GameObject textObj, TextMeshPro textMesh)
    {
        float duration = 2.0f; // エフェクトの持続時間
        float elapsedTime = 0f;
        Vector3 originalScale = textObj.transform.localScale;

        // 初期位置を設定
        textObj.transform.position = new Vector3(-68.75f, 251.52f, 0f);

        while (elapsedTime < duration)
        {
            // 拡大
            textObj.transform.localScale = Vector3.Lerp(originalScale, originalScale * 2, elapsedTime / duration);

            // フェードアウト
            Color color = textMesh.color;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            textMesh.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(textObj); // テキストオブジェクトを削除
    }
    private bool CheckIfValidCompound(out List<ElementType> matchedCompound)
    {
        var elementCounts = new Dictionary<ElementType, int>();
        foreach (var piece in connectedPieces)
        {
            if (piece != null)
            {
                if (!elementCounts.ContainsKey(piece.elementType))
                {
                    elementCounts[piece.elementType] = 0;
                }
                elementCounts[piece.elementType]++;
            }
        }

        foreach (var combination in compoundScores.Keys)
        {
            var tempCounts = new Dictionary<ElementType, int>(elementCounts);

            bool isMatch = true;
            foreach (var element in combination)
            {
                if (tempCounts.ContainsKey(element))
                {
                    tempCounts[element]--;
                    if (tempCounts[element] == 0)
                    {
                        tempCounts.Remove(element);
                    }
                }
                else
                {
                    isMatch = false;
                    break;
                }
            }

            if (isMatch && tempCounts.Count == 0)
            {
                matchedCompound = combination;
                return true;
            }
        }

        matchedCompound = null;
        return false;
    }

    public void DestroyWithEffect()
    {
        // エフェクトを生成する
        if (sparkleEffectPrefab != null)
        {
            GameObject effect = Instantiate(sparkleEffectPrefab, transform.position, Quaternion.identity);
            
            // パーティクルシステムを取得し再生
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }

            // 一定時間後にエフェクトオブジェクトを削除
            Destroy(effect, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);

            
        }

        // 自身の削除
        Destroy(gameObject);

        // 自身に接続されたすべての青い線を削除
        List<LineRenderer> linesToRemove = new List<LineRenderer>();
        foreach (var line in allLines)
        {
            if (line != null && (line.GetPosition(0) == transform.position || line.GetPosition(1) == transform.position))
            {
                linesToRemove.Add(line);
            }
        }
        foreach (var line in linesToRemove)
        {
            allLines.Remove(line);
            Destroy(line.gameObject);
        }
    }



    private Vector2Int GetGridPosition()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    internal void SetInteractive(bool enable)
    {
        throw new NotImplementedException();
    }

}