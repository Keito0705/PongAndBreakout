using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldBlock : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // SpriteRenderer��ێ����邽�߂̕ϐ�


    // Start is called before the first frame update
    void Start()
    {
        // SpriteRenderer�R���|�[�l���g���擾
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        }

        if (collision.gameObject.name == "BlackBall")
        {
            // �F�����ɕύX
            spriteRenderer.color = Color.white;

            // ���C���[��BlackBlock�ɕύX
            gameObject.layer = LayerMask.NameToLayer("WhiteBlock");
        }
    }
}
