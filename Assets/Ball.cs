using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D myRigid;

    public float speedX = 10;
    public float speedY = 10;

    // 最小速度
    public float minSpeed = 75;
    // 最大速度
    public float maxSpeed = 1000;

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
}
