using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public enum WallType { Left, Right }
    public WallType wallType; // Inspector�őI��
    public BlockPlacer blockPlacer; // Inspector�ŃZ�b�g

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �^�O�����ł����A���O�����OK
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
