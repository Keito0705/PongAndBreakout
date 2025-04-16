using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhitePaddle : MonoBehaviour
{
    //WhitePaddle

    private Rigidbody2D myRigid;

    public float speed = 1.0f;
    public float followThreshold = 0.3f; // 追いかける反応の閾値（これを大きくすると追いかける動きが鈍くなる）

    [Header("CPU操作にする場合はtrue")]
    public bool isCPU = false;

    [Header("CPU時に追いかけるボール")]
    public Transform ball; // Inspectorでボールを指定


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
