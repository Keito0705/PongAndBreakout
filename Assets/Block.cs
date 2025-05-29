using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public int row;
    public int col;
    public BlockPlacer placer;


    [Header("赤枠線のPrefab（Inspectorでセット）")]
    public GameObject redOutlinePrefab;

    private GameObject redOutlineInstance; // 実際に生成した枠線
    private Coroutine blinkCoroutine;      // 点滅コルーチン管理用

    public static Block Instance; // シングルトン

    [Header("現在の温度値")]
    public float temperature1 = 0;
    public float temperature2 = 0;

   
    public void ReflectionTemperature1(float amount)
    {
        temperature1 = amount;
        if (temperature1 >= 100 && placer != null)
        {
            if (blinkCoroutine == null) // 二重起動防止
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


    // 点滅→爆発コルーチン
    private IEnumerator BlinkAndExplode(BlockPlacer.BlockColor fromColor)
    {
        // 赤枠線生成
        if (redOutlinePrefab != null && redOutlineInstance == null)
        {
            redOutlineInstance = Instantiate(redOutlinePrefab, transform);
            redOutlineInstance.transform.localPosition = Vector3.zero;
            redOutlineInstance.transform.SetParent(transform); // 親子関係を設定
        }

        float blinkTime = 1.0f;      // 点滅総時間（秒）
        float blinkInterval = 0.15f; // 点滅間隔（秒）
        float timer = 0f;
        bool isActive = false;



        while (timer < blinkTime)
        {
            if (redOutlineInstance != null)
                redOutlineInstance.SetActive(isActive = !isActive);
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // 最後に枠線を消す
        if (redOutlineInstance != null)
            Destroy(redOutlineInstance);

        // 爆発
        placer.ExplodeAround(row, col, 3, fromColor);

        temperature1 = 0;
        temperature2 = 0;

        blinkCoroutine = null;
    }

  
    //通常時
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (placer == null) return;

        //左プレイヤー用
        if (collision.gameObject.CompareTag("WhiteBall") && temperature1 < 100)
        {
            placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.Black, "BlackBlock");
        }
        //右プレイヤー用
        else if (collision.gameObject.CompareTag("BlackBall") && temperature2 < 100)
        {
            placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.White, "WhiteBlock");
        }
    }
}