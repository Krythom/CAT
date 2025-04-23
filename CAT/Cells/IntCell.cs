using Microsoft.Xna.Framework;

namespace CAT;

public class IntCell : Cell
{
    public int Strength;
    
    public IntCell(int x, int y, int strength, Color col)
    {
        Strength = strength;
        Pos = new Point(x, y);
        Col = col;
    }
}