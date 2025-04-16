using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePaddle : MonoBehaviour
{
    //WhitePaddle

    private Rigidbody2D myRigid;

    public float speed = 1.0f;
    public float followThreshold = 0.3f; // �ǂ������锽����臒l�i�����傫������ƒǂ������铮�����݂��Ȃ�j

    [Header("CPU����ɂ���ꍇ��true")]
    public bool isCPU = false;

    [Header("CPU���ɒǂ�������{�[��")]
    public Transform ball; // Inspector�Ń{�[�����w��


    // Start is called before the first frame update
    void Start()
    {
        myRigid = this.GetComponent<Rigidbody2D>();
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
            if (Input.GetKey(KeyCode.W))
            {
                force = new Vector2(0, speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                force = new Vector2(0, -speed);
            }
        }
        myRigid.MovePosition(myRigid.position + force * Time.fixedDeltaTime);
    }
}
