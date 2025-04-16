using UnityEngine;

public class Block : MonoBehaviour
{
    public int row;
    public int col;
    public BlockPlacer placer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (placer == null) return;

        if (collision.gameObject.name == "WhiteBall")
        {
            placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.Black, "BlackBlock");
        }
        else if (collision.gameObject.name == "BlackBall")
        {
            placer.SetBlockColorAndLayer(row, col, BlockPlacer.BlockColor.White, "WhiteBlock");
        }
    }
}
