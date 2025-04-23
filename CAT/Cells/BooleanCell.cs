using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace CAT;

public class BooleanCell : Cell
{
    public bool Alive;
    
    public BooleanCell(int x, int y, bool living)
    {
        Alive = living;
        Pos = new Point(x, y);
        Col = living ? Color.White : Color.Black;
    }
}