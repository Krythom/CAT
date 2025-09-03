using System;
using System.Collections.Generic;
using System.DirectoryServices;
using SharpDX;
using Color = Microsoft.Xna.Framework.Color;

namespace CAT;

public class Pond(int types, int tolerance) : Iterator
{
    private List<IntCell> _neighbors = [];
    private IntCell[,] _world;
    private IntCell[,] _newWorld;
    private int _width;
    private int _height;
    
    public override IntCell[,] InitWorld(int width, int height)
    {
        _width = width;
        _height = height;
        _world = new IntCell[_width, _height];
        _newWorld = new IntCell[_width, _height];
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int type = Rand.Next(types);
                _world[x, y] = new IntCell(x, y, type, Color.White);
            }
        }
        
        return _world;
    }

    public override IntCell[,] Iterate()
    {
        bool done = true;
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                IntCell current = _world[x, y];
                _neighbors = current.GetMoore(_world, 1, true, _neighbors);
                bool active = false;

                foreach (IntCell neighbor in _neighbors)
                {
                    int diff = Math.Abs(current.Id - neighbor.Id);
                    if (diff < tolerance)
                    {
                        active = true;
                        break;
                    }
                }

                if (active)
                {
                    done = false;
                    int newStrength = (current.Id + Rand.Next(-1, 2) + types) % types;
                    IntCell placed = new(x, y, newStrength, Color.White);
                    placed.LastUpdate = Cat.Iterations;
                    placed.Updates = current.Updates + 1;
                    _newWorld[x, y] = placed;
                }
                else
                {
                    _newWorld[x, y] = current;
                    _newWorld[x, y].Col = Color.Black;
                }
            }
        }

        if (done)
        {
            Completed = true;
        }
        
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}