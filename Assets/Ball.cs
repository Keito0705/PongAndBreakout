using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D myRigid;

    public float speedX = 10;
    public float speedY = 10;

    // �ő呬�x
    public float minSpeed = 75;

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
        // ���݂̑��x���擾
        Vector2 velocity = myRigid.velocity;


        //X�����̑��x���`�F�b�N(�{����)
        if (0 <= velocity.x && velocity.x < minSpeed)
        {
            myRigid.velocity = new Vector2(minSpeed, velocity.y);
        }
        //X�����̑��x���`�F�b�N(�|����)
        if (0 >= velocity.x && velocity.x > -minSpeed)
        {
            myRigid.velocity = new Vector2(-minSpeed, velocity.y);
        }

        //Y�����̑��x���`�F�b�N(�{����)
        if (0 <= velocity.y && velocity.y < minSpeed)
        {
            myRigid.velocity = new Vector2(velocity.x, minSpeed);
        }
        //Y�����̑��x���`�F�b�N(�|����)
        if (0 >= velocity.y && velocity.y > -minSpeed)
        {
            myRigid.velocity = new Vector2(velocity.x, -minSpeed);
        }
    }
}
