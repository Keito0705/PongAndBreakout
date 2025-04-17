using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public enum WallType { Left, Right }
    public WallType wallType; // Inspectorで選択
    public BlockPlacer blockPlacer; // Inspectorでセット

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // タグ推奨ですが、名前判定もOK
        if (wallType == WallType.Right && (collision.gameObject.name == "BlackBall" || collision.gameObject.CompareTag("BlackBall")))
        {
            blockPlacer.ShiftRight();
            blockPlacer.UpdateBlocks();
        }
        else if (wallType == WallType.Left && (collision.gameObject.name == "WhiteBall" || collision.gameObject.CompareTag("WhiteBall")))
        {
            blockPlacer.ShiftLeft();
            blockPlacer.UpdateBlocks();
        }
    }
}
