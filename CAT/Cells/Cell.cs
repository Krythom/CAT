using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CAT;

public abstract class Cell
{
    public Point Pos;
    public Color Col;
    public string Name;
    public int Updates;
    public int LastUpdate;
    
    public List<T> GetMoore<T>(T[,] world, int range, bool wrap, List<T> neighbors) where T : Cell
    {
        neighbors.Clear();
        int worldX = world.GetLength(0);
        int worldY = world.GetLength(1);

        for (int x = -1 * range; x <= range; x++)
        {
            for (int y = -1 * range; y <= range; y++)
            {
                if (wrap)
                {
                    if (!(x == 0 && y == 0))
                    {
                        neighbors.Add(world[(Pos.X + x + worldX) % worldX, (Pos.Y + y + worldY) % worldY]);
                    }
                }
                else
                {
                    if (!(x == 0 && y == 0) && BoundsCheck(worldX, worldY, Pos.X + x, Pos.Y + y))
                    {
                        neighbors.Add(world[Pos.X + x, Pos.Y + y]);
                    }
                }
            }
        }
        
        return neighbors;
    }
    
    public List<T> GetNeumann<T>(T[,] world, int range, bool wrap, List<T> neighbors) where T : Cell
    {
        neighbors.Clear();
        int worldX = world.GetLength(0);
        int worldY = world.GetLength(1);

        for (int x = -1 * range; x <= range; x++)
        {
            if (wrap)
            {
                if (x != 0)
                {
                    neighbors.Add(world[(Pos.X + x + worldX) % worldX, Pos.Y]);
                }
            }
            else
            {
                if (x != 0 && BoundsCheck(worldX, worldY, Pos.X + x, Pos.Y))
                {
                    neighbors.Add(world[Pos.X + x, Pos.Y]);
                }
            }
        }
        for (int y = -1 * range; y <= range; y++)
        {
            if (wrap)
            {
                if (y != 0)
                {
                    neighbors.Add(world[Pos.X, (Pos.Y + y + worldY) % worldY]);
                }
            }
            else
            {
                if (y != 0 && BoundsCheck(worldX, worldY, Pos.X, Pos.Y + y))
                {
                    neighbors.Add(world[Pos.X, Pos.Y + y]);
                }
            }
        }
        
        return neighbors;
    }

    public T GetCell<T>(T[,] world, int xOffset, int yOffset, bool wrap) where T : Cell
    {
        int worldX = world.GetLength(0);
        int worldY = world.GetLength(1);

        if (wrap)
        {
            return world[(Pos.X + xOffset + worldX) % worldX, (Pos.Y + yOffset + worldY) % worldY];
        }

        if (Pos.X + xOffset >= 0 && Pos.Y + yOffset >= 0 && Pos.X + xOffset < worldX && Pos.Y + yOffset < worldY)
        {
            return world[Pos.X + xOffset, Pos.Y + yOffset];
        }

        return null;
    }

    private bool BoundsCheck(int width, int height, int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}