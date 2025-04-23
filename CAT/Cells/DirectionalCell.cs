using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CAT;

public class DirectionalCell : Cell
{
    public double Strength { get; set; }
    public Point Direction { get; set; }
    
    public DirectionalCell(int x, int y, Point direction, Color col)
    {
        Direction = direction;
        Pos = new Point(x, y);
        Col = col;
    }

    public DirectionalCell(int x, int y, Point direction, double strength, Color col)
    {
        Direction = direction;
        Strength = strength;
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
    
    public static readonly List<Point> Cardinals = [Up, Right, Down, Left];

    public static Point GetDir(int n)
    {
        return n switch
        {
            0 => Up,
            1 => Right,
            2 => Down,
            3 => Left,
            _ => None
        };
    }

    public static int GetInt(Point p)
    {
        if (p == Up)
        {
            return 0;
        }
        if (p == Right)
        {
            return 1;
        }
        if (p == Down)
        {
            return 2;
        }
        if (p == Left)
        {
            return 3;
        }
        return 4;
    }
}