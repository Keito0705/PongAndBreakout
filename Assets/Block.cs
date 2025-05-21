using UnityEngine;

public class Block : MonoBehaviour
{
    public int row;
    public int col;
    public BlockPlacer placer;

    public static Block Instance; // シングルトン

    [Header("現在の温度値")]
    public float temperature1 = 0;
    public float temperature2 = 0;

    //void Awake()
    //{
    //    Instance = this;
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (placer == null) return;

        //左プレイヤー用
        if (collision.gameObject.CompareTag("WhiteBall"))
        {
            if (temperature1 < 50) //温度が100に到達いてない時は色変えのみ
            {
                placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.Black, "BlackBlock");
            }
            else //温度が100なら大爆発
            {

            }
        }
        //右プレイヤー用
        else if (collision.gameObject.CompareTag("BlackBall"))
        {
            if (temperature2 < 50) //温度が100に到達いてない時は色変えのみ
            {
                placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.White, "WhiteBlock");
            }
            else //温度が100なら大爆発
            {

            }
        }
    }

    public void ReflectionTemperature1(float amount)
    {
        //温度を受け取る

        //temperature1 = amount;
        //Debug.Log($"温度が{temperature1}");
    }

    public void ReflectionTemperature2(float amount)
    {
        //温度を受け取る

        //temperature2 = amount;
    }
}
