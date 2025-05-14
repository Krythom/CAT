using Microsoft.Xna.Framework;

namespace CAT;

public class Rug : Cell
{
    public int State;

    public Rug(int x, int y, int state, Color color)
    {
        Pos = new Point(x, y);
        State = state;
        Col = color;
    }
}