using Microsoft.Xna.Framework;

namespace CAT;

public class Molecule : Cell
{
    
    public Molecule(int x, int y, Color col)
    {
        Pos = new Point(x, y);
        Col = col;
    }
}