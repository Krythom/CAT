using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CAT;

public class GoL : Iterator
{
    private BooleanCell[,] _world;
    private BooleanCell[,] _newWorld;
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
        List<BooleanCell> neighbors = [];
        if (Cat.Iterations == 10000)
        {
            Completed = true;
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                BooleanCell current = _world[x, y];
                neighbors = current.GetMoore(_world, 1, true, neighbors);
                
                int aliveNeighbors = neighbors.Count(neighbor => neighbor.Alive);

                if (current.Alive)
                {
                    if (aliveNeighbors is > 3 or < 2)
                    {
                        _newWorld[x, y] = new BooleanCell(x, y, false);
                        _newWorld[x, y].Updates = current.Updates + 1;
                        _newWorld[x, y].LastUpdate = Cat.Iterations;
                    }
                    else
                    {
                        _newWorld[x, y] = current;
                    }
                }
                else
                {
                    if (aliveNeighbors == 3)
                    {
                        _newWorld[x, y] = new BooleanCell(x, y, true);
                        _newWorld[x, y].Updates = current.Updates + 1;
                        _newWorld[x, y].LastUpdate = Cat.Iterations;
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