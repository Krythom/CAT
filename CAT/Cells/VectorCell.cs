using Microsoft.Xna.Framework;

namespace CAT;

public class VectorCell : Cell
{
    public Vector2 Vector;
    public bool Locked;
    
    public VectorCell(int x, int y, Vector2 vec, Color col)
    {
        Vector = vec;
        Pos = new Point(x, y);
        Col = col;
        Locked = false;
    }
}