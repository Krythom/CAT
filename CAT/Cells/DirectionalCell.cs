using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CAT;

public class DirectionalCell : Cell
{
    public double Strength { get; set; }
    public Point Dir { get; set; }
    public int Index { get; set; }
    
    public DirectionalCell(int x, int y, int index, Color col)
    {
        Index = index;
        Dir = Direction.Cardinals[index];
        Pos = new Point(x, y);
        Col = col;
    }
    
    public DirectionalCell(int x, int y, Color col)
    {
        Index = -1;
        Dir = Direction.None;
        Pos = new Point(x, y);
        Col = col;
    }
}

public static class Direction
{
    public static readonly Point Up = new(0, -1);
    public static readonly Point Down = new(0, 1);
    public static readonly Point Left = new(-1, 0);
    public static readonly Point Right = new(1, 0);
    public static readonly Point None = new(0, 0);
    
    public static readonly Point[] Cardinals = [Up, Right, Down, Left];
}