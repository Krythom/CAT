using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace CAT;

public class WallCell : Cell
{
    public bool Alive;
    public bool DarkRegion;
    public bool Last;
    
    public WallCell(int x, int y, bool living)
    {
        Alive = living;
        Pos = new Point(x, y);
        Name = "BooleanCell";
        Col = living ? Color.White : Color.Black;
    }
}