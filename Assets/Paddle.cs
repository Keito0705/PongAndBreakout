using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    //WhitePaddle

  

    public bool isCPU = false; //CPU操作かどうかを指定
    public Transform ball; // Inspectorでボールを指定
    public float speed = 1.0f;
    public float followThreshold = 0.3f; // 追いかける反応の閾値（これを大きくすると追いかける動きが鈍くなる）
   

    // パドルがどちら側にあるか（右側なら1、左側なら-1）
    public int paddleSide = 1; // 右側:1, 左側:-1

    private Rigidbody2D myRigid;
    public Collider2D paddleCollider; // Inspectorでセット（またはGetComponentで取得）
    private Collider2D ballCollider;

    private bool ignoringCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        if (paddleCollider == null)
            paddleCollider = GetComponent<Collider2D>();
        if (ball != null)
            ballCollider = ball.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 force = Vector2.zero;

        if (isCPU)
        {
            if (ball != null)
            {
                // ボールのY座標と自分のY座標の差を計算
                float direction = ball.position.y - transform.position.y;

                // ある程度の閾値を設けて、細かく動きすぎないようにする
                if (Mathf.Abs(direction) > followThreshold)
                {
                    force = new Vector2(0, Mathf.Sign(direction) * speed);
                }
            }
        }
        else
        {
            // 左パドル（paddleSide == -1）はW/Sキー、右パドル（paddleSide == 1）は↑↓キー
            if (paddleSide == 1)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    force = new Vector2(0, speed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    force = new Vector2(0, -speed);
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    force = new Vector2(0, speed);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    force = new Vector2(0, -speed);
                }
            }
        }
        myRigid.MovePosition(myRigid.position + force * Time.fixedDeltaTime);




        // ボールが自ゴール側から来ているか判定
        if (ball != null && paddleCollider != null)
        {
            if (ballCollider == null)
                ballCollider = ball.GetComponent<Collider2D>();

            Rigidbody2D ballRigid = ball.GetComponent<Rigidbody2D>();
            if (ballRigid != null)
            {
                bool fromOwnGoal = (paddleSide == 1 && ballRigid.velocity.x > 0) ||
                                   (paddleSide == -1 && ballRigid.velocity.x < 0);

                if (fromOwnGoal && !ignoringCollision)
                {
                    Physics2D.IgnoreCollision(paddleCollider, ballCollider, true);
                    ignoringCollision = true;
                }
                else if (!fromOwnGoal && ignoringCollision)
                {
                    Physics2D.IgnoreCollision(paddleCollider, ballCollider, false);
                    ignoringCollision = false;
                }
            }
        }
    }
}
