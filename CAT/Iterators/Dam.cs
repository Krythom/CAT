using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.Xna.Framework;

namespace CAT;

public class Dam(int types) : Iterator
{
    private List<IntCell> _neighbors = [];
    private Color[] _cols;
    private IntCell[,] _world;
    private IntCell[,] _newWorld;
    private int _width;
    private int _height;
    
    public override IntCell[,] InitWorld(int width, int height)
    {
        _world = new IntCell[height, width];
        _newWorld = new IntCell[height, width];
        _cols = new Color[types];
        
        for (int i = 0; i < types; i++)
        {
            _cols[i] = new Color(Rand.Next(256), Rand.Next(256), Rand.Next(256));
        }
        
        _width = width;
        _height = height;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int type = Rand.Next(types);
                _world[x, y] = new IntCell(x,y, type, _cols[type]);
            }
        }
        
        return _world;
    }

    public override IntCell[,] Iterate()
    {
        for (int i = 0; i < _cols.Length; i++)
        {
            Color c = _cols[i];
            _cols[i] = new Color(c.R + Rand.Next(-1,2), c.G + Rand.Next(-1,2), c.B + Rand.Next(-1,2));
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                IntCell current = _world[x, y];
                int[] counts = new int[types];
                List<int> valid = [];
                int next = current.Strength;
                _neighbors = current.GetMoore(_world, 1, true, _neighbors);
                
                foreach (IntCell neighbor in _neighbors)
                {
                    counts[neighbor.Strength]++;
                }
                
                for (int i = 0; i < types; i++)
                {
                    if (counts[i] == 3)
                    {
                        valid.Add(i);
                    }
                }
                
                if (valid.Count > 0)
                {
                    next = valid[Rand.Next(valid.Count)];
                }

                if (next != current.Strength)
                {
                    _newWorld[x, y] = new IntCell(x, y, next, _cols[next])
                    {
                        LastUpdate = Cat.Iterations,
                        Updates = current.Updates + 1
                    };
                }
                else
                {
                    _newWorld[x, y] = current;
                }
            }
        }
        
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}