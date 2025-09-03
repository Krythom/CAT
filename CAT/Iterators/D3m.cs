using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CAT;

public class D3m : Iterator
{
    private List<IntCell> _neighbors = [];
    private readonly Color[] _cols = [Color.White, Color.Black, Color.Blue, Color.Red, Color.Green];
    private IntCell[,] _world;
    private IntCell[,] _newWorld;
    private int _width;
    private int _height;
    
    public override IntCell[,] InitWorld(int width, int height)
    {
        _world = new IntCell[width, height];
        _newWorld = new IntCell[width, height];
        
        _width = width;
        _height = height;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                double r = Rand.NextDouble();
                if (r < 0.00005)
                {
                    _world[x, y] = new IntCell(x,y, 4, _cols[4]);
                }
                else
                {
                    _world[x, y] = new IntCell(x,y, 0, _cols[0]);
                }
            }
        }
        
        return _world;
    }

    public override IntCell[,] Iterate()
    {
        Completed = true;
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                IntCell current = _world[x, y];
                int next = current.Id;
                    
                switch (current.Id)
                {
                    case 4:
                        Completed = false;
                        next = 1;
                        break;
                    
                    case 3:
                        Completed = false;
                        next = 2;
                        break;
                    
                    case 2:
                    {
                        Completed = false;
                        int walls = 0;

                        _neighbors = current.GetMoore(_world, 2, true, _neighbors);
                        foreach (IntCell neighbor in _neighbors)
                        {
                            if (neighbor.Id == 1)
                            {
                                walls++;
                            }
                        }

                        if (walls > Rand.Next(40) + 4 || walls == 4)
                        {
                            next = 4;
                        }
                        else
                        {
                            next = 0;
                        }
                        
                        break;
                    }
                    
                    case 0:
                    {
                        _neighbors = current.GetMoore(_world, 1, true, _neighbors);
                        foreach (IntCell neighbor in _neighbors)
                        {
                            if (neighbor.Id is 3 or 4)
                            {
                                next = 3;
                                break;
                            }
                        }

                        break;
                    }
                }

                if (next == current.Id)
                {
                    _newWorld[x, y] = current;
                }
                else
                {
                    _newWorld[x, y] = new IntCell(x, y, next, _cols[next])
                    {
                        LastUpdate = Cat.Iterations,
                        Updates = current.Updates + 1
                    };
                }
            }
        }
        
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}