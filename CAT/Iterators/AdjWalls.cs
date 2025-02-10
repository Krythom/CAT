using System.Collections.Generic;
using System.Linq;

namespace CAT;

public class AdjWalls : Iterator
{
    private BooleanCell[,] _world;
    private BooleanCell[,] _newWorld;
    private List<BooleanCell> _neighbors = [];
    private int _width;
    private int _height;
    
    public override BooleanCell[,] InitWorld(int width, int height)
    {
        _width = width;
        _height = height;
        _world = new BooleanCell[width, height];
        _newWorld = new BooleanCell[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Rand.Next(2) == 0)
                {
                    _world[x, y] = new BooleanCell(x, y, false);
                }
                else
                {
                    _world[x, y] = new BooleanCell(x, y, true);
                }
            }
        }

        return _world;
    }
    
    public override BooleanCell[,] Iterate()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                BooleanCell current = _world[x, y];

                if (current.Alive)
                {
                    _neighbors = current.GetMoore(_world, 1, true, _neighbors);
                    int count = _neighbors.Count(n => n.Alive);
                    if (count > 3)
                    {
                        _newWorld[x, y] = new BooleanCell(x, y, false);
                    }
                    else
                    {
                        _newWorld[x, y] = current;
                    }
                }
                else
                {
                    _neighbors = current.GetNeumann(_world, 1, true, _neighbors);
                    int count = _neighbors.Count(n => n.Alive);
                    if (count > 1)
                    {
                        _newWorld[x, y] = new BooleanCell(x, y, true);
                    }
                    else
                    {
                        _newWorld[x, y] = current;
                    }
                }
            }
        }
        
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}