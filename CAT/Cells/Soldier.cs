using Microsoft.Xna.Framework;

namespace CAT;

public class Soldier : Cell
{
    public int Id;
    public double Count;

    public Soldier(int x, int y, int id, double count, Color col)
    {
        Id = id;
        Count = count;
        Pos = new Point(x, y);
        Col = col;
    }
}