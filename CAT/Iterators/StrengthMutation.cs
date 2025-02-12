using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CAT;

public class StrengthMutation(int types) : Iterator
{
    private FloatCell[,] _world;
    private FloatCell[,] _newWorld;
    private List<FloatCell> _neighbors = [];
    private readonly Color[] _colors = new Color[types];
    private int _width;
    private int _height;
        
    public override FloatCell[,] InitWorld(int width, int height)
    {
        _world = new FloatCell[width, height];
        _newWorld = new FloatCell[width, height];
        _width = width;
        _height = height;

        for (int i = 0; i < types; i++)
        {
            _colors[i] = new Color(Rand.Next(255), Rand.Next(255), Rand.Next(255));
        }
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int id = Rand.Next(types);
                _world[x,y] = new FloatCell(x, y, Rand.NextDouble(), id, _colors[id]);
            }
        }
        
        return _world;
    }

    public override FloatCell[,] Iterate()
    {
        bool done = true;
        
        for (int i = 0; i < _colors.Length; i++)
        {
            _colors[i] = Mutate(_colors[i], 5);
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                FloatCell current = _world[x, y];
                _neighbors = current.GetMoore(_world, 1, true, _neighbors);

                double highest = double.MinValue;
                FloatCell winner = current;
            
                foreach (FloatCell neighbor in _neighbors)
                {
                    if (neighbor.Strength > highest)
                    {
                        highest = neighbor.Strength;
                        winner = neighbor;
                    }

                    if (neighbor.Id != current.Id)
                    {
                        done = false;
                    }
                }
                
                if (winner.Id == current.Id)
                {
                    FloatCell newCell = new(x, y, highest, winner.Id, current.Col)
                    {
                        LastUpdate = current.LastUpdate,
                        Updates = current.Updates
                    };
                    _newWorld[x, y] = newCell;
                }
                else
                {
                    FloatCell newCell = new(x, y, highest, winner.Id, _colors[winner.Id])
                    {
                        LastUpdate = Cat.Iterations,
                        Updates = current.Updates + 1
                    };
                    _newWorld[x, y] = newCell;
                }
            
                _newWorld[x, y].Strength += Rand.NextDouble() - 1;
            }
        } 
        
        (_newWorld, _world) = (_world, _newWorld);
        Completed = done;
        return _world;
    }
}