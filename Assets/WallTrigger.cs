using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public enum WallType { Left, Right }
    public WallType wallType; // Inspector�őI��
    public BlockPlacer blockPlacer; // Inspector�ŃZ�b�g
    public Scenechange scenechange; // Inspector�ŃZ�b�g

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //// �^�O�����ł����A���O�����OK
        //�~�X�������̍��E���炵
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

        //����̕ǂɃ^�b�`�����珟��
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
