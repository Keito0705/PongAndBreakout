using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public GameObject blockPrefab; // 白色のPrefabのみ
    public int rows = 5;
    public int columns = 10; // 片側の列数（全体はcolumns*2）
    public float spacing = 1.5f;
    public float initializePosx = 0.25f;
    public float initializePosy = 0.25f;

    private GameObject[,] blocks;
    private BlockColor[,] colorArray;
    private string[,] layerArray;

    public enum BlockColor { Black, White }

    void Start()
    {
        int totalCols = columns * 2;
        blocks = new GameObject[rows, totalCols];
        colorArray = new BlockColor[rows, totalCols];
        layerArray = new string[rows, totalCols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < totalCols; j++)
            {
                Vector3 position = new Vector3(j * spacing + initializePosx, -i * spacing + initializePosy, 0);
                blocks[i, j] = Instantiate(blockPrefab, position, Quaternion.identity);

                Block blockScript = blocks[i, j].GetComponent<Block>();
                if (blockScript != null)
                {
                    blockScript.row = i;
                    blockScript.col = j;
                    blockScript.placer = this;
                }

                if (j < columns)
                {
                    colorArray[i, j] = BlockColor.Black;
                    layerArray[i, j] = "BlackBlock";
                }
                else
                {
                    colorArray[i, j] = BlockColor.White;
                    layerArray[i, j] = "WhiteBlock";
                }
            }
        }
        UpdateBlocks();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShiftRight();
            UpdateBlocks();
        }
    }

    public void SetBlockColorAndLayer(int row, int col, BlockColor color, string layerName)
    {
        colorArray[row, col] = color;
        layerArray[row, col] = layerName;
        SetBlockColorAndLayer(blocks[row, col], color, layerName);
    }

    void ShiftRight()
    {
        int totalCols = columns * 2;
        BlockColor[,] newColorArray = new BlockColor[rows, totalCols];
        string[,] newLayerArray = new string[rows, totalCols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = totalCols - 1; j >= 0; j--)
            {
                if (j == 0)
                {
                    newColorArray[i, j] = BlockColor.Black;
                    newLayerArray[i, j] = "BlackBlock";
                }
                else
                {
                    newColorArray[i, j] = colorArray[i, j - 1];
                    newLayerArray[i, j] = layerArray[i, j - 1];
                }
            }
        }
        colorArray = newColorArray;
        layerArray = newLayerArray;
    }

    void UpdateBlocks()
    {
        int totalCols = columns * 2;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < totalCols; j++)
            {
                SetBlockColorAndLayer(blocks[i, j], colorArray[i, j], layerArray[i, j]);
            }
        }
    }

    void SetBlockColorAndLayer(GameObject block, BlockColor color, string layerName)
    {
        var spriteRenderer = block.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = (color == BlockColor.Black) ? Color.black : Color.white;
        }
        int layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
        {
            block.layer = layer;
        }
    }
}
