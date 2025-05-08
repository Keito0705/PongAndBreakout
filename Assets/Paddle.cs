using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isCPU = false;
    public Transform ball;
    //public float speed = 8.0f;
    public float speedX = 2.0f; // 左右移動速度
    public float speedY = 8.0f; // 上下移動速度
    public float followThreshold = 0.8f;
    public int paddleSide = 1; // 右側:1, 左側:-1

    private Rigidbody2D myRigid;
    public Collider2D paddleCollider;
    private Collider2D ballCollider;
    private bool ignoringCollision = false;

    // 角度調整用
    public float maxAngle = 20f;
    private int angleState = 0; // 0:中立, 1:上向き, -1:下向き

    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        if (paddleCollider == null)
            paddleCollider = GetComponent<Collider2D>();
        if (ball != null)
            ballCollider = ball.GetComponent<Collider2D>();
        ApplyAngle();
    }

    void Update()
    {
        // シフトキーで角度切り替え（左パドル:RightShift、右パドル:LeftShift）
        if ((paddleSide == 1 && Input.GetKeyDown(KeyCode.LeftShift)) ||
            (paddleSide == -1 && Input.GetKeyDown(KeyCode.RightShift)))
        {
            angleState = angleState switch { 0 => 1, 1 => -1, _ => 0 };
            ApplyAngle();
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = Vector2.zero;

        if (isCPU)
        {
            if (ball != null)
            {
                float direction = ball.position.y - transform.position.y;
                if (Mathf.Abs(direction) > followThreshold)
                {
                    movement.y = Mathf.Sign(direction) * speedY;
                }
            }
        }
        else
        {
            // 右パドル操作（WASD）
            //if (paddleSide == 1)
            //{
            //    if (Input.GetKey(KeyCode.W)) movement.y = speed;
            //    if (Input.GetKey(KeyCode.S)) movement.y = -speed;
            //    if (Input.GetKey(KeyCode.D)) movement.x = speed;
            //    if (Input.GetKey(KeyCode.A)) movement.x = -speed;
            //}
            //// 左パドル操作（矢印キー）
            //else
            //{
            //    if (Input.GetKey(KeyCode.UpArrow)) movement.y = speed;
            //    if (Input.GetKey(KeyCode.DownArrow)) movement.y = -speed;
            //    if (Input.GetKey(KeyCode.RightArrow)) movement.x = speed;
            //    if (Input.GetKey(KeyCode.LeftArrow)) movement.x = -speed;
            //}

            if (paddleSide == 1)
            {
                if (Input.GetKey(KeyCode.D)) movement.x = speedX;
                if (Input.GetKey(KeyCode.A)) movement.x = -speedX;
                if (Input.GetKey(KeyCode.W)) movement.y = speedY;
                if (Input.GetKey(KeyCode.S)) movement.y = -speedY;
            }
            // 左パドル操作（矢印キー）
            else
            {
                if (Input.GetKey(KeyCode.RightArrow)) movement.x = speedX;
                if (Input.GetKey(KeyCode.LeftArrow)) movement.x = -speedX;
                if (Input.GetKey(KeyCode.UpArrow))   movement.y = speedY;
                if (Input.GetKey(KeyCode.DownArrow)) movement.y = -speedY;
            }
        }
        myRigid.MovePosition(myRigid.position + movement * Time.fixedDeltaTime);

        HandleBallCollision();
    }

    void ApplyAngle()
    {
        float targetAngle = angleState switch
        {
            1 => maxAngle,
            -1 => -maxAngle,
            _ => 0f
        };
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }

    void HandleBallCollision()
    {
        if (ball == null || paddleCollider == null) return;

        if (ballCollider == null)
            ballCollider = ball.GetComponent<Collider2D>();

        Rigidbody2D ballRigid = ball.GetComponent<Rigidbody2D>();
        if (ballRigid == null) return;

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
