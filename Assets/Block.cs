using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // SpriteRenderer��ێ����邽�߂̕ϐ�

    // Start is called before the first frame update
    void Start()
    {
       // SpriteRenderer�R���|�[�l���g���擾
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ������Ԃ�WhiteBlock���C���[��ݒ�i�K�v�Ȃ�ǉ��j
        //gameObject.layer = LayerMask.NameToLayer("WhiteBlock");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "WhiteBall")
        {
            // �F�����ɕύX
            spriteRenderer.color = Color.black;

            // ���C���[��BlackBlock�ɕύX
            gameObject.layer = LayerMask.NameToLayer("BlackBlock");

            //Destroy(this.gameObject);
        }
        if (collision.gameObject.name == "BlackBall")
        {
            // �F�����ɕύX
            spriteRenderer.color = Color.white;

            // ���C���[��BlackBlock�ɕύX
            gameObject.layer = LayerMask.NameToLayer("WhiteBlock");

            //Destroy(this.gameObject);
        }
    }
}
