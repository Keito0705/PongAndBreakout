using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isCPU = false; // CPU操作かどうか
    public Transform ball;     // Inspectorでボールを指定
    public float speed = 1.0f;
    public float followThreshold = 0.3f; // 追いかける反応の閾値
    public int paddleSide = 1; // 右側:1, 左側:-1

    // パドル切り替え用
    public GameObject rectPaddle;        // 長方形パドル (WhitePaddle/BlackPaddle)
    public GameObject halfCirclePaddle;  // 半円パドル (WhiteHalfCircle/BlackHalfCircle)
    private bool isHalfCircle = false;

    private Rigidbody2D myRigid;
    private Collider2D paddleCollider;
    private Collider2D ballCollider;
    private bool ignoringCollision = false;

    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();

        // 最初は長方形パドルON、半円パドルOFF
        if (rectPaddle != null) rectPaddle.SetActive(true);
        if (halfCirclePaddle != null) halfCirclePaddle.SetActive(false);
        isHalfCircle = false;

        // コライダーを長方形から取得
        if (rectPaddle != null)
            paddleCollider = rectPaddle.GetComponent<Collider2D>();
        if (ball != null)
            ballCollider = ball.GetComponent<Collider2D>();
    }

    void Update()
    {
        // パドル形状切り替え
        // 左パドル（paddleSide == -1）は右キー、右パドル（paddleSide == 1）はAキー
        if ((paddleSide == -1 && Input.GetKeyDown(KeyCode.RightArrow)) ||
            (paddleSide == 1 && Input.GetKeyDown(KeyCode.A)))
        {
            isHalfCircle = !isHalfCircle;
            if (rectPaddle != null) rectPaddle.SetActive(!isHalfCircle);
            if (halfCirclePaddle != null) halfCirclePaddle.SetActive(isHalfCircle);

            // コライダーも切り替え
            if (isHalfCircle && halfCirclePaddle != null)
                paddleCollider = halfCirclePaddle.GetComponent<Collider2D>();
            else if (rectPaddle != null)
                paddleCollider = rectPaddle.GetComponent<Collider2D>();
        }
    }

    void FixedUpdate()
    {
        Vector2 force = Vector2.zero;

        if (isCPU)
        {
            if (ball != null)
            {
                float direction = ball.position.y - transform.position.y;
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
                    force = new Vector2(0, speed);
                if (Input.GetKey(KeyCode.S))
                    force = new Vector2(0, -speed);
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    force = new Vector2(0, speed);
                if (Input.GetKey(KeyCode.DownArrow))
                    force = new Vector2(0, -speed);
            }
        }
        // 親オブジェクト（WhitePaddleParent/BlackPaddleParent）を動かす
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
