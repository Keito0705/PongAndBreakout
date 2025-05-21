using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public enum WallType { Left, Right }
    public WallType wallType; // Inspectorで選択
    public BlockPlacer blockPlacer; // Inspectorでセット
    public Scenechange scenechange; // Inspectorでセット

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //// タグ推奨ですが、名前判定もOK
        //ミスった時の左右ずらし
        if (wallType == WallType.Right && (collision.gameObject.name == "BlackBall" || collision.gameObject.CompareTag("BlackBall")))
        {
            //scenechange.FromToResult();
            blockPlacer.ShiftRight();
            blockPlacer.UpdateBlocks();
        }
        else if (wallType == WallType.Left && (collision.gameObject.name == "WhiteBall" || collision.gameObject.CompareTag("WhiteBall")))
        {
            //scenechange.FromToResult();
            blockPlacer.ShiftLeft();
            blockPlacer.UpdateBlocks();
        }

        //相手の壁にタッチしたら勝ち
        if (wallType == WallType.Right && (collision.gameObject.name == "WhiteBall" || collision.gameObject.CompareTag("WhiteBall")))
        {
            scenechange.FromToResult();
        }
        else if (wallType == WallType.Left && (collision.gameObject.name == "BlackBall" || collision.gameObject.CompareTag("BlackBall")))
        {
            scenechange.FromToResult();
        }

    }
}
