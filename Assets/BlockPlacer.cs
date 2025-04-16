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

    // �ǂ����Prefab�����L�^����z��itrue:��, false:���j
    private bool[,] isBlackBlock;
    // ���ۂ̃u���b�N�I�u�W�F�N�g
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
                // �����͍��������A�E������
                isBlackBlock[i, j] = (j < columns / 2);
            }
        }
        UpdateBlockColors(); // ������Ԃ̐F�ƃ��C���[�����f
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShiftColorsRight();
            UpdateBlockColors();
        }
    }

    // �F��񂾂����E�ɃV�t�g
    void ShiftColorsRight()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = columns - 1; j >= 0; j--)
            {
                if (j == 0)
                {
                    isBlackBlock[i, j] = true; // ��ԍ��͍�
                }
                else
                {
                    isBlackBlock[i, j] = isBlackBlock[i, j - 1];
                }
            }
        }
    }

    // �z��̏��Ɋ�Â��ĐF�ƃ��C���[���X�V
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
            Debug.LogWarning($"Layer '{layerName}' �����݂��܂���B");
        }
    }
}
