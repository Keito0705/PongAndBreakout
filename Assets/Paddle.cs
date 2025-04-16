using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    //WhitePaddle

  

    public bool isCPU = false; //CPU���삩�ǂ������w��
    public Transform ball; // Inspector�Ń{�[�����w��
    public float speed = 1.0f;
    public float followThreshold = 0.3f; // �ǂ������锽����臒l�i�����傫������ƒǂ������铮�����݂��Ȃ�j
   

    // �p�h�����ǂ��瑤�ɂ��邩�i�E���Ȃ�1�A�����Ȃ�-1�j
    public int paddleSide = 1; // �E��:1, ����:-1

    private Rigidbody2D myRigid;
    public Collider2D paddleCollider; // Inspector�ŃZ�b�g�i�܂���GetComponent�Ŏ擾�j
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
                // �{�[����Y���W�Ǝ�����Y���W�̍����v�Z
                float direction = ball.position.y - transform.position.y;

                // ������x��臒l��݂��āA�ׂ������������Ȃ��悤�ɂ���
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
