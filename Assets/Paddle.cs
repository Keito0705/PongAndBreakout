using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isCPU = false;
    public Transform ball;
    public float speed = 8.0f;
    public float followThreshold = 0.8f;
    public int paddleSide = 1; // 右側:1, 左側:-1

    private Rigidbody2D myRigid;
    public Collider2D paddleCollider;
    private Collider2D ballCollider;
    private bool ignoringCollision = false;

    // 角度調整用
    public float angleSpeed = 200f;
    public float minAngle = -20f;
    public float maxAngle = 20f;
    private float currentAngle = 0f;
    private bool isAngleAdjustMode = false;
    private bool angleLockAtZero = false;

    // タイミング打ち用
    public float hitOffset = 0.5f;      // パドルがズレる距離
    public float hitDuration = 0.08f;   // ズレている時間（秒）
    public float hitPower = 8.0f;       // ボールに与える加速力
    public float hitMoveTime = 0.06f;   // ズレる・戻るアニメーションの時間
    private bool isHitting = false;
    private Vector3 originalPosition;   // ゲーム開始時の初期位置
    private Vector3 basePosition;       // ズレる直前の位置

    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        if (paddleCollider == null)
            paddleCollider = GetComponent<Collider2D>();
        if (ball != null)
            ballCollider = ball.GetComponent<Collider2D>();
        currentAngle = 0f;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        originalPosition = transform.position;
    }

    void Update()
    {
        // 角度調整モード判定
        if (paddleSide == 1)
        {
            isAngleAdjustMode = Input.GetKey(KeyCode.A);
        }
        else
        {
            isAngleAdjustMode = Input.GetKey(KeyCode.RightArrow);
        }

        // タイミング打ち入力
        // 左パドル（paddleSide == -1）は左キー、右パドル（paddleSide == 1）はDキー
        if (!isHitting)
        {
            if ((paddleSide == -1 && Input.GetKeyDown(KeyCode.LeftArrow)) ||
                (paddleSide == 1 && Input.GetKeyDown(KeyCode.D)))
            {
                StartCoroutine(HitAction());
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 force = Vector2.zero;

        // CPU操作
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
        // プレイヤー操作
        else
        {
            if (isAngleAdjustMode)
            {
                // 角度調整
                float angleDelta = 0f;
                bool keyPressed = false;

                if (paddleSide == 1)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        angleDelta = angleSpeed * Time.fixedDeltaTime;
                        keyPressed = true;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        angleDelta = -angleSpeed * Time.fixedDeltaTime;
                        keyPressed = true;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        angleDelta = -angleSpeed * Time.fixedDeltaTime;
                        keyPressed = true;
                    }
                    if (Input.GetKey(KeyCode.DownArrow))
                    {
                        angleDelta = angleSpeed * Time.fixedDeltaTime;
                        keyPressed = true;
                    }
                }

                // 角度調整ロック判定
                if (!angleLockAtZero)
                {
                    float nextAngle = Mathf.Clamp(currentAngle + angleDelta, minAngle, maxAngle);

                    bool willCrossZero = (currentAngle > 0f && nextAngle <= 0f) || (currentAngle < 0f && nextAngle >= 0f);
                    if (willCrossZero && Mathf.Abs(nextAngle) < 0.5f && keyPressed)
                    {
                        currentAngle = 0f;
                        angleLockAtZero = true;
                        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                        return;
                    }
                    else if (keyPressed)
                    {
                        currentAngle = nextAngle;
                        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
                    }
                }

                // ロック解除（キーを離したら）
                bool keyReleased;
                if (paddleSide == 1)
                {
                    keyReleased = !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S);
                }
                else
                {
                    keyReleased = !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow);
                }
                if (angleLockAtZero && keyReleased)
                {
                    angleLockAtZero = false;
                }
            }
            else
            {
                // 通常の上下移動
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

    // 連続的にズレて戻るアニメーションを行うコルーチン
    IEnumerator HitAction()
    {
        isHitting = true;

        // ズレる直前の位置を保存（上下移動中でもOK）
        basePosition = transform.position;

        // ズレる方向を決定
        // 左パドル（paddleSide == -1）は右方向、右パドル（paddleSide == 1）は左方向
        Vector3 offset = (paddleSide == -1 ? -transform.right : transform.right) * hitOffset;

        // スムーズにズレる
        float t = 0f;
        while (t < hitMoveTime)
        {
            float rate = t / hitMoveTime;
            transform.position = Vector3.Lerp(basePosition, basePosition + offset, rate);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = basePosition + offset;

        // ズレた位置でしばらく待機
        yield return new WaitForSeconds(hitDuration);

        // スムーズに元の位置へ戻る
        t = 0f;
        while (t < hitMoveTime)
        {
            float rate = t / hitMoveTime;
            transform.position = Vector3.Lerp(basePosition + offset, basePosition, rate);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = basePosition;

        isHitting = false;
    }

    // タイミング打ち中にボールと当たったら加速
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHitting && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                // 左パドルは右方向、右パドルは左方向
                Vector2 hitDir = (paddleSide == -1 ? transform.right : -transform.right);
                ballRb.velocity += hitDir.normalized * hitPower;
            }
        }
    }
}
