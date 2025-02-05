using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace CAT;

public class BooleanCell : Cell
{
    public readonly bool Alive;
    
    public BooleanCell(int x, int y, bool living)
    {
        Alive = living;
        Pos = new Point(x, y);
        Name = "BooleanCell";
        Col = living ? Color.White : Color.Black;
    }
}