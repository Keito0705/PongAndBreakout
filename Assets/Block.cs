using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public int row;
    public int col;
    public BlockPlacer placer;


    [Header("�Ԙg����Prefab�iInspector�ŃZ�b�g�j")]
    public GameObject redOutlinePrefab;

    private GameObject redOutlineInstance; // ���ۂɐ��������g��
    private Coroutine blinkCoroutine;      // �_�ŃR���[�`���Ǘ��p

    public static Block Instance; // �V���O���g��

    [Header("���݂̉��x�l")]
    public float temperature1 = 0;
    public float temperature2 = 0;

   
    public void ReflectionTemperature1(float amount)
    {
        temperature1 = amount;
        if (temperature1 >= 100 && placer != null)
        {
            if (blinkCoroutine == null) // ��d�N���h�~
                blinkCoroutine = StartCoroutine(BlinkAndExplode(BlockPlacer.BlockColor.White));            
        }
    }

    public void ReflectionTemperature2(float amount)
    {
        temperature2 = amount;
        if (temperature2 >= 100 && placer != null)
        {
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(BlinkAndExplode(BlockPlacer.BlockColor.Black));
        }
    }


    // �_�Ł������R���[�`��
    private IEnumerator BlinkAndExplode(BlockPlacer.BlockColor fromColor)
    {
        // �Ԙg������
        if (redOutlinePrefab != null && redOutlineInstance == null)
        {
            redOutlineInstance = Instantiate(redOutlinePrefab, transform);
            redOutlineInstance.transform.localPosition = Vector3.zero;
            redOutlineInstance.transform.SetParent(transform); // �e�q�֌W��ݒ�
        }

        float blinkTime = 1.0f;      // �_�ő����ԁi�b�j
        float blinkInterval = 0.15f; // �_�ŊԊu�i�b�j
        float timer = 0f;
        bool isActive = false;



        while (timer < blinkTime)
        {
            if (redOutlineInstance != null)
                redOutlineInstance.SetActive(isActive = !isActive);
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // �Ō�ɘg��������
        if (redOutlineInstance != null)
            Destroy(redOutlineInstance);

        // ����
        placer.ExplodeAround(row, col, 3, fromColor);

        temperature1 = 0;
        temperature2 = 0;

        blinkCoroutine = null;
    }

  
    //�ʏ펞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (placer == null) return;

        //���v���C���[�p
        if (collision.gameObject.CompareTag("WhiteBall") && temperature1 < 100)
        {
            placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.Black, "BlackBlock");
        }
        //�E�v���C���[�p
        else if (collision.gameObject.CompareTag("BlackBall") && temperature2 < 100)
        {
            placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.White, "WhiteBlock");
        }
    }
}