using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blackBlockPrefab;
    public GameObject whiteBlockPrefab;
    public int rows = 5;
    public int columns = 10;
    public float spacing = 1.5f;
    public float initializePosx = 0.25f;
    public float initializePosy = 0.25f;

    // どちらのPrefabかを記録する配列（true:黒, false:白）
    private bool[,] isBlackBlock;
    // 実際のブロックオブジェクト
    private GameObject[,] blocks;

    void Start()
    {
        isBlackBlock = new bool[rows, columns];
        blocks = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(j * spacing + initializePosx, -i * spacing + initializePosy, 0);
                GameObject prefab = (j < columns / 2) ? blackBlockPrefab : whiteBlockPrefab;
                blocks[i, j] = Instantiate(prefab, position, Quaternion.identity, transform);
                // 初期は左半分黒、右半分白
                isBlackBlock[i, j] = (j < columns / 2);
            }
        }
        UpdateBlockColors(); // 初期状態の色とレイヤーも反映
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShiftColorsRight();
            UpdateBlockColors();
        }
    }

    // 色情報だけを右にシフト
    void ShiftColorsRight()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = columns - 1; j >= 0; j--)
            {
                if (j == 0)
                {
                    isBlackBlock[i, j] = true; // 一番左は黒
                }
                else
                {
                    isBlackBlock[i, j] = isBlackBlock[i, j - 1];
                }
            }
        }
    }

    // 配列の情報に基づいて色とレイヤーを更新
    void UpdateBlockColors()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var block = blocks[i, j];
                if (block != null)
                {
                    if (isBlackBlock[i, j])
                    {
                        SetBlockColorAndLayer(block, Color.black, "BlackBlock");
                    }
                    else
                    {
                        SetBlockColorAndLayer(block, Color.white, "WhiteBlock");
                    }
                }
            }
        }
    }

    void SetBlockColorAndLayer(GameObject block, Color color, string layerName)
    {
        var renderer = block.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
        {
            block.layer = layer;
        }
        else
        {
            Debug.LogWarning($"Layer '{layerName}' が存在しません。");
        }
    }
}
