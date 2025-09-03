using Microsoft.Xna.Framework;

namespace CAT;

public class IntCell : Cell
{
    public int Id;
    
    public IntCell(int x, int y, int id, Color col)
    {
        Id = id;
        Pos = new Point(x, y);
        Col = col;
    }
}