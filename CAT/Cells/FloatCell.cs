using Microsoft.Xna.Framework;

namespace CAT;

public class FloatCell : Cell
{
    public double Strength;
    public int Id;
    
    public FloatCell(int x, int y, double strength, int id, Color col)
    {
        Strength = strength;
        Pos = new Point(x, y);
        Col = col;
        Id = id;
    }
}