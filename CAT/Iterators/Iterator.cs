using System;
using Microsoft.Xna.Framework;

namespace CAT;

public abstract class Iterator()
{
    public bool Completed = false;
    public static Random Rand;

    public abstract Cell[,] InitWorld(int width, int height);
    
    public abstract Cell[,] Iterate();

    protected static Color Mutate(Color c, int strength)
    {
        return new Color(c.R + Rand.Next(-strength, strength + 1),
                        c.G + Rand.Next(-strength, strength + 1), 
                        c.B + Rand.Next(-strength, strength + 1));
    }
}