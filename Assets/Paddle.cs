using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isCPU = false; // CPU���삩�ǂ���
    public Transform ball;     // Inspector�Ń{�[�����w��
    public float speed = 1.0f;
    public float followThreshold = 0.3f; // �ǂ������锽����臒l
    public int paddleSide = 1; // �E��:1, ����:-1

    // �p�h���؂�ւ��p
    public GameObject rectPaddle;        // �����`�p�h�� (WhitePaddle/BlackPaddle)
    public GameObject halfCirclePaddle;  // ���~�p�h�� (WhiteHalfCircle/BlackHalfCircle)
    private bool isHalfCircle = false;

    private Rigidbody2D myRigid;
    private Collider2D paddleCollider;
    private Collider2D ballCollider;
    private bool ignoringCollision = false;

    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();

        // �ŏ��͒����`�p�h��ON�A���~�p�h��OFF
        if (rectPaddle != null) rectPaddle.SetActive(true);
        if (halfCirclePaddle != null) halfCirclePaddle.SetActive(false);
        isHalfCircle = false;

        // �R���C�_�[�𒷕��`����擾
        if (rectPaddle != null)
            paddleCollider = rectPaddle.GetComponent<Collider2D>();
        if (ball != null)
            ballCollider = ball.GetComponent<Collider2D>();
    }

    void Update()
    {
        // �p�h���`��؂�ւ�
        // ���p�h���ipaddleSide == -1�j�͉E�L�[�A�E�p�h���ipaddleSide == 1�j��A�L�[
        if ((paddleSide == -1 && Input.GetKeyDown(KeyCode.RightArrow)) ||
            (paddleSide == 1 && Input.GetKeyDown(KeyCode.A)))
        {
            isHalfCircle = !isHalfCircle;
            if (rectPaddle != null) rectPaddle.SetActive(!isHalfCircle);
            if (halfCirclePaddle != null) halfCirclePaddle.SetActive(isHalfCircle);

            // �R���C�_�[���؂�ւ�
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
            // ���p�h���ipaddleSide == -1�j��W/S�L�[�A�E�p�h���ipaddleSide == 1�j�́����L�[
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
        // �e�I�u�W�F�N�g�iWhitePaddleParent/BlackPaddleParent�j�𓮂���
        myRigid.MovePosition(myRigid.position + force * Time.fixedDeltaTime);

        // �{�[�������S�[�������痈�Ă��邩����
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
