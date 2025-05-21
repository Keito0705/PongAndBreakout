using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D myRigid;

    public float speedX = 10;
    public float speedY = 10;

    //反射スピード？
    public float reflectionSpeed = 2;

    // 最小速度
    public float minSpeed = 2;
    // 最大速度
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
        // 現在の速度を取得
        Vector2 velocity = myRigid.velocity;


        ////低速度チェック
        //X方向の速度をチェック(＋方向)
        if (0 <= velocity.x && velocity.x < minSpeed)
        {
            myRigid.velocity = new Vector2(minSpeed, velocity.y);
        }
        //X方向の速度をチェック(－方向)
        if (0 >= velocity.x && velocity.x > -minSpeed)
        {
            myRigid.velocity = new Vector2(-minSpeed, velocity.y);
        }
        //Y方向の速度をチェック(＋方向)
        if (0 <= velocity.y && velocity.y < minSpeed)
        {
            myRigid.velocity = new Vector2(velocity.x, minSpeed);
        }
        //Y方向の速度をチェック(－方向)
        if (0 >= velocity.y && velocity.y > -minSpeed)
        {
            myRigid.velocity = new Vector2(velocity.x, -minSpeed);
        }



        ////高速度チェック
        //X方向の速度をチェック(＋方向)
        if (maxSpeed <= velocity.x)
        {
            myRigid.velocity = new Vector2(maxSpeed, velocity.y);
        }
        //X方向の速度をチェック(－方向)
        if (-maxSpeed >= velocity.x)
        {
            myRigid.velocity = new Vector2(-maxSpeed, velocity.y);
        }

        //Y方向の速度をチェック(＋方向)
        if (maxSpeed <= velocity.y)
        {
            myRigid.velocity = new Vector2(velocity.x, maxSpeed);
        }
        //Y方向の速度をチェック(－方向)
        if (-maxSpeed >= velocity.y)
        {
            myRigid.velocity = new Vector2(velocity.x, -maxSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // パドルに当たったとき、自前で反射させる
            Vector2 reflectDir = Vector2.Reflect(myRigid.velocity.normalized, collision.contacts[0].normal);
            myRigid.velocity = reflectDir * reflectionSpeed;
        }
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // パドルに当たったとき、自前で反射させる
            Vector2 reflectDir = Vector2.Reflect(myRigid.velocity.normalized, collision.contacts[0].normal);
            myRigid.velocity = reflectDir * reflectionSpeed;
        }

        //ブロックに当たった時、温度を上昇させる
        if (collision.gameObject.CompareTag("Block"))
        {
            //左プレイヤー用
            if (this.gameObject.name == "WhiteBall")
            {
                temperature += addTemperature;
                if (temperature > MaxTemperature) { temperature = MaxTemperature; }
                GaugeManager.Instance.ReflectionTemperature1(temperature);
                Block.Instance.ReflectionTemperature1(temperature);
            }
            //右プレイヤー用
            if (this.gameObject.name == "BlackBall")
            {
                temperature += addTemperature;
                //
                if (temperature > MaxTemperature) { temperature = MaxTemperature; }
                GaugeManager.Instance.ReflectionTemperature2(temperature);
                Block.Instance.ReflectionTemperature2(temperature);
            }
        }

        //壁に当たった時、温度を０にする(左用)
        if(collision.gameObject.name == "WallL" && this.gameObject.name == "WhiteBall")
        {
            temperature = 0.0f;
            GaugeManager.Instance.ReflectionTemperature1(temperature);
            Block.Instance.ReflectionTemperature1(temperature);
        }
        //壁に当たった時、温度を０にする(左用)
        if (collision.gameObject.name == "WallR" && this.gameObject.name == "BlockBall")
        {
            temperature = 0.0f;
            GaugeManager.Instance.ReflectionTemperature2(temperature);
            Block.Instance.ReflectionTemperature2(temperature);
        }

    }
}
