using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isCPU = false;
    public Transform ball;
    public float speed = 8.0f;
    public float followThreshold = 0.8f;
    public int paddleSide = 1; // �E��:1, ����:-1

    private Rigidbody2D myRigid;
    public Collider2D paddleCollider;
    private Collider2D ballCollider;
    private bool ignoringCollision = false;

    // �p�x�����p
    public float angleSpeed = 200f;
    public float minAngle = -20f;
    public float maxAngle = 20f;
    private float currentAngle = 0f;
    private bool isAngleAdjustMode = false;
    private bool angleLockAtZero = false;

    // �^�C�~���O�ł��p
    public float hitOffset = 0.5f;      // �p�h�����Y���鋗��
    public float hitDuration = 0.08f;   // �Y���Ă��鎞�ԁi�b�j
    public float hitPower = 8.0f;       // �{�[���ɗ^���������
    public float hitMoveTime = 0.06f;   // �Y����E�߂�A�j���[�V�����̎���
    private bool isHitting = false;
    private Vector3 originalPosition;   // �Q�[���J�n���̏����ʒu
    private Vector3 basePosition;       // �Y���钼�O�̈ʒu

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
        // �p�x�������[�h����
        if (paddleSide == 1)
        {
            isAngleAdjustMode = Input.GetKey(KeyCode.A);
        }
        else
        {
            isAngleAdjustMode = Input.GetKey(KeyCode.RightArrow);
        }

        // �^�C�~���O�ł�����
        // ���p�h���ipaddleSide == -1�j�͍��L�[�A�E�p�h���ipaddleSide == 1�j��D�L�[
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

        // CPU����
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
        // �v���C���[����
        else
        {
            if (isAngleAdjustMode)
            {
                // �p�x����
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

                // �p�x�������b�N����
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

                // ���b�N�����i�L�[�𗣂�����j
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
                // �ʏ�̏㉺�ړ�
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

    // �A���I�ɃY���Ė߂�A�j���[�V�������s���R���[�`��
    IEnumerator HitAction()
    {
        isHitting = true;

        // �Y���钼�O�̈ʒu��ۑ��i�㉺�ړ����ł�OK�j
        basePosition = transform.position;

        // �Y�������������
        // ���p�h���ipaddleSide == -1�j�͉E�����A�E�p�h���ipaddleSide == 1�j�͍�����
        Vector3 offset = (paddleSide == -1 ? -transform.right : transform.right) * hitOffset;

        // �X���[�Y�ɃY����
        float t = 0f;
        while (t < hitMoveTime)
        {
            float rate = t / hitMoveTime;
            transform.position = Vector3.Lerp(basePosition, basePosition + offset, rate);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = basePosition + offset;

        // �Y�����ʒu�ł��΂炭�ҋ@
        yield return new WaitForSeconds(hitDuration);

        // �X���[�Y�Ɍ��̈ʒu�֖߂�
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

    // �^�C�~���O�ł����Ƀ{�[���Ɠ������������
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isHitting && collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                // ���p�h���͉E�����A�E�p�h���͍�����
                Vector2 hitDir = (paddleSide == -1 ? transform.right : -transform.right);
                ballRb.velocity += hitDir.normalized * hitPower;
            }
        }
    }
}
