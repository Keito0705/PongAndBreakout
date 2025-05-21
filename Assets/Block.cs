using UnityEngine;

public class Block : MonoBehaviour
{
    public int row;
    public int col;
    public BlockPlacer placer;

    public static Block Instance; // �V���O���g��

    [Header("���݂̉��x�l")]
    public float temperature1 = 0;
    public float temperature2 = 0;

    //void Awake()
    //{
    //    Instance = this;
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (placer == null) return;

        //���v���C���[�p
        if (collision.gameObject.CompareTag("WhiteBall"))
        {
            if (temperature1 < 50) //���x��100�ɓ��B���ĂȂ����͐F�ς��̂�
            {
                placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.Black, "BlackBlock");
            }
            else //���x��100�Ȃ�唚��
            {

            }
        }
        //�E�v���C���[�p
        else if (collision.gameObject.CompareTag("BlackBall"))
        {
            if (temperature2 < 50) //���x��100�ɓ��B���ĂȂ����͐F�ς��̂�
            {
                placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.White, "WhiteBlock");
            }
            else //���x��100�Ȃ�唚��
            {

            }
        }
    }

    public void ReflectionTemperature1(float amount)
    {
        //���x���󂯎��

        //temperature1 = amount;
        //Debug.Log($"���x��{temperature1}");
    }

    public void ReflectionTemperature2(float amount)
    {
        //���x���󂯎��

        //temperature2 = amount;
    }
}
