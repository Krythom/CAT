using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CAT;

public class RugWorld : Iterator
{
    private const int Mask = 0xBD;
    private int _width;
    private int _height;
    private Rug[,] _world;
    private Rug[,] _newWorld;
    private Color[] _colors;
    private List<Rug> _neighbors = [];
    
    public override Rug[,] InitWorld(int width, int height)
    {
        _width = width;
        _height = height;
        _world = new Rug[_width, _height];
        _newWorld = new Rug[_width, _height];
        _colors = new Color[256];

        int r = Rand.Next(128);
        int g = Rand.Next(128);
        int b = Rand.Next(128);
        
        for (int i = 0; i < 256; i++)
        {
            _colors[i] = new Color(r, g, b);
            r += Rand.Next(-10, 11) % 256;
            g += Rand.Next(-10, 11) % 256;
            b += Rand.Next(-10, 11) % 256;
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int state = Rand.Next(256);
                _world[x, y] = new Rug(x, y, state, _colors[state]);
            }
        }
        
        return _world;
    }

    public override Rug[,] Iterate()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Rug current = _world[x, y];
                _neighbors = current.GetMoore(_world, 1, true, _neighbors);
                
                int avg = 0;
                foreach (Rug neighbor in _neighbors)
                {
                    avg += neighbor.State;
                }
                
                avg /= 8;
                avg = (avg + 10) % 256; 
                avg ^= Mask;
                _newWorld[x, y] = new Rug(x, y, avg, _colors[avg]);
            }
        }
        
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}