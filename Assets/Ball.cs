using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D myRigid;

    public float speedX = 10;
    public float speedY = 10;

    //���˃X�s�[�h�H
    public float reflectionSpeed = 2;

    // �ŏ����x
    public float minSpeed = 2;
    // �ő呬�x
    public float maxSpeed = 16;

    private float temperature = 0;
    private float addTemperature = 5;
    private float MaxTemperature = 100;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();

        Vector2 force = new Vector2(speedX, speedY);

        myRigid.AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {
        // ���݂̑��x���擾
        Vector2 velocity = myRigid.velocity;


        ////�ᑬ�x�`�F�b�N
        //X�����̑��x���`�F�b�N(�{����)
        if (0 <= velocity.x && velocity.x < minSpeed)
        {
            myRigid.velocity = new Vector2(minSpeed, velocity.y);
        }
        //X�����̑��x���`�F�b�N(�|����)
        if (0 >= velocity.x && velocity.x > -minSpeed)
        {
            myRigid.velocity = new Vector2(-minSpeed, velocity.y);
        }
        //Y�����̑��x���`�F�b�N(�{����)
        if (0 <= velocity.y && velocity.y < minSpeed)
        {
            myRigid.velocity = new Vector2(velocity.x, minSpeed);
        }
        //Y�����̑��x���`�F�b�N(�|����)
        if (0 >= velocity.y && velocity.y > -minSpeed)
        {
            myRigid.velocity = new Vector2(velocity.x, -minSpeed);
        }



        ////�����x�`�F�b�N
        //X�����̑��x���`�F�b�N(�{����)
        if (maxSpeed <= velocity.x)
        {
            myRigid.velocity = new Vector2(maxSpeed, velocity.y);
        }
        //X�����̑��x���`�F�b�N(�|����)
        if (-maxSpeed >= velocity.x)
        {
            myRigid.velocity = new Vector2(-maxSpeed, velocity.y);
        }

        //Y�����̑��x���`�F�b�N(�{����)
        if (maxSpeed <= velocity.y)
        {
            myRigid.velocity = new Vector2(velocity.x, maxSpeed);
        }
        //Y�����̑��x���`�F�b�N(�|����)
        if (-maxSpeed >= velocity.y)
        {
            myRigid.velocity = new Vector2(velocity.x, -maxSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // �p�h���ɓ��������Ƃ��A���O�Ŕ��˂�����
            Vector2 reflectDir = Vector2.Reflect(myRigid.velocity.normalized, collision.contacts[0].normal);
            myRigid.velocity = reflectDir * reflectionSpeed;
        }
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // �p�h���ɓ��������Ƃ��A���O�Ŕ��˂�����
            Vector2 reflectDir = Vector2.Reflect(myRigid.velocity.normalized, collision.contacts[0].normal);
            myRigid.velocity = reflectDir * reflectionSpeed;
        }

        //�u���b�N�ɓ����������A���x���㏸������
        if (collision.gameObject.CompareTag("Block"))
        {
            //���v���C���[�p
            if (this.gameObject.name == "WhiteBall")
            {
                temperature += addTemperature;
                if (temperature > MaxTemperature) { temperature = MaxTemperature; }
                GaugeManager.Instance.ReflectionTemperature1(temperature);
                Block.Instance.ReflectionTemperature1(temperature);
            }
            //�E�v���C���[�p
            if (this.gameObject.name == "BlackBall")
            {
                temperature += addTemperature;
                //
                if (temperature > MaxTemperature) { temperature = MaxTemperature; }
                GaugeManager.Instance.ReflectionTemperature2(temperature);
                Block.Instance.ReflectionTemperature2(temperature);
            }
        }

        //�ǂɓ����������A���x���O�ɂ���(���p)
        if(collision.gameObject.name == "WallL" && this.gameObject.name == "WhiteBall")
        {
            temperature = 0.0f;
            GaugeManager.Instance.ReflectionTemperature1(temperature);
            Block.Instance.ReflectionTemperature1(temperature);
        }
        //�ǂɓ����������A���x���O�ɂ���(���p)
        if (collision.gameObject.name == "WallR" && this.gameObject.name == "BlockBall")
        {
            temperature = 0.0f;
            GaugeManager.Instance.ReflectionTemperature2(temperature);
            Block.Instance.ReflectionTemperature2(temperature);
        }

    }
}
