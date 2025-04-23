using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CAT;

public class Crystal : Iterator
{
    private int _width;
    private int _height;
    private double _temp = 0f;
    private List<VectorCell> _neighbors = new();
    private VectorCell[,] _world;
    
    public override VectorCell[,] InitWorld(int width, int height)
    {
        _width = width;
        _height = height;
        _world = new VectorCell[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2 vec = Vector2.UnitX;
                vec.Rotate(Rand.Next());
                Color col = new(vec.X, (vec.Y + vec.X)/4, vec.Y);
                _world[x, y] = new VectorCell(x, y, vec, col);
            }
        }

        return _world;
    }

    public override VectorCell[,] Iterate()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                VectorCell current = _world[x, y];
                
                if (!current.Locked)
                {
                    float instability = 0;
                    _neighbors = current.GetMoore(_world, 1, true, _neighbors);

                    foreach (VectorCell neighbor in _neighbors)
                    {
                        float dot = 1 - Vector2.Dot(current.Vector, neighbor.Vector);
                        instability += dot;
                    }
                    
                    double rotation = instability * _temp; 
                    current.Vector.Rotate((float)rotation); 
                    current.LastUpdate = Cat.Iterations; 
                    
                    double angle = Math.Abs(Math.Atan(current.Vector.Y/current.Vector.X));
                    if (current.Vector.X < 0)
                    {
                        if (current.Vector.Y < 0)
                        {
                            angle += Math.PI;
                        }
                        else
                        {
                            angle += Math.PI/2;
                        }
                    }
                    else
                    {
                        if (current.Vector.Y < 0)
                        {
                            angle += 3*Math.PI/2;
                        }
                    }
                    
                    current.Updates = (int)(angle * 360);
                    current.Col = new Color(current.Vector.X, (current.Vector.X + current.Vector.Y) / 2, current.Vector.Y);
                }
            }
        }

        if (_temp < 1)
        {
            _temp += 0.0001;
        }

        if (_temp <= 0)
        {
            Completed = true;
        }
        
        return _world;
    }
}