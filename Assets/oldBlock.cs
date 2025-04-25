using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldBlock : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // SpriteRendererを保持するための変数


    // Start is called before the first frame update
    void Start()
    {
        // SpriteRendererコンポーネントを取得
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
            // 色を黒に変更
            spriteRenderer.color = Color.black;

            // レイヤーをBlackBlockに変更
            gameObject.layer = LayerMask.NameToLayer("BlackBlock");
        }

        if (collision.gameObject.name == "BlackBall")
        {
            // 色を黒に変更
            spriteRenderer.color = Color.white;

            // レイヤーをBlackBlockに変更
            gameObject.layer = LayerMask.NameToLayer("WhiteBlock");
        }
    }
}
