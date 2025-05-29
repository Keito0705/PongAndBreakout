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

    public static GaugeManager Instance; // �V���O���g��

    [Header("�e�v���C���[�̃Q�[�WUI")]
    public Slider gauge1;
    public Slider gauge2;

    [Header("�Q�[�W�ő�l")]
    public float maxGauge = 90f;

    [Header("���݂̃Q�[�W�l")]
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

    // �v���C���[1�̃Q�[�W�𔽉f������
    public void ReflectionTemperature1(float amount)
    {
        gaugeValue1 = amount;
        gauge1.value = gaugeValue1;

        //Debug.Log($"���x��{gaugeValue1}");
    }

    // �v���C���[2�̃Q�[�W�𔽉f������
    public void ReflectionTemperature2(float amount)
    {
        gaugeValue2 = amount;
        gauge2.value = gaugeValue2;

        //Debug.Log($"���x��{gaugeValue2}");
    }
}
