using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;

namespace CAT;

public class AdjWalls : Iterator
{
    private WallCell[,] _world;
    private WallCell[,] _newWorld;
    private List<WallCell> _neighbors = [];
    private int _width;
    private int _height;
    
    public override WallCell[,] InitWorld(int width, int height)
    {
        _width = width;
        _height = height;
        _world = new WallCell[width, height];
        _newWorld = new WallCell[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Rand.Next(2) == 0)
                {
                    _world[x, y] = new WallCell(x, y, false);
                }
                else
                {
                    _world[x, y] = new WallCell(x, y, true);
                }
            }
        }

        return _world;
    }
    
    public override WallCell[,] Iterate()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                WallCell current = _world[x, y];

                if (current.Alive)
                {
                    _neighbors = current.GetMoore(_world, 1, true, _neighbors);
                    int count = _neighbors.Count(n => n.Alive);
                    if (count > 3)
                    {
                        _newWorld[x, y] = new WallCell(x, y, false);
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
                        _newWorld[x, y] = new WallCell(x, y, true);
                    }
                    else
                    {
                        _newWorld[x, y] = current;
                    }

                    if (Cat.Iterations % 2 == 0)
                    {
                        _neighbors = _newWorld[x, y].GetMoore(_world, 1, true, _neighbors);
                        int alive = 1 + _neighbors.Count(neighbor => neighbor.Alive);
                        _newWorld[x, y].DarkRegion = alive <= 4;

                        if (_newWorld[x, y].DarkRegion != current.DarkRegion)
                        {
                            _newWorld[x, y].Updates = current.Updates + 1;
                            _newWorld[x, y].LastUpdate = Cat.Iterations;
                        }
                    }
                    
                    _newWorld[x, y].Col = _newWorld[x, y].DarkRegion? Color.Black : Color.White;
                }
            }
        }

        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}