using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GaugeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GaugeManager Instance; // シングルトン

    [Header("各プレイヤーのゲージUI")]
    public Slider gauge1;
    public Slider gauge2;

    [Header("ゲージ最大値")]
    public float maxGauge = 90f;

    [Header("現在のゲージ値")]
    public float gaugeValue1 = 0f;
    public float gaugeValue2 = 0f;

    void Awake()
    {
        Instance = this;
        gauge1.minValue = 0f;
        gauge1.maxValue = maxGauge;
        gauge1.value = 0f;
        gauge2.minValue = 0f;
        gauge2.maxValue = maxGauge;
        gauge2.value = 0f;
    }

    // プレイヤー1のゲージを反映させる
    public void ReflectionTemperature1(float amount)
    {
        gaugeValue1 = amount;
        gauge1.value = gaugeValue1;

        //Debug.Log($"温度が{gaugeValue1}");
    }

    // プレイヤー2のゲージを反映させる
    public void ReflectionTemperature2(float amount)
    {
        gaugeValue2 = amount;
        gauge2.value = gaugeValue2;

        //Debug.Log($"温度が{gaugeValue2}");
    }
}
