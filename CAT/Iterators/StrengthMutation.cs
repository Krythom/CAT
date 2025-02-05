using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CAT;

public class StrengthMutation(int types) : Iterator
{
    private FloatCell[,] _world;
    private FloatCell[,] _newWorld;
    List<FloatCell> _neighbors = [];
    private readonly Color[] _colors = new Color[types];
    private readonly HashSet<Point> _checkSet = [];
    private readonly HashSet<Point> _toSleep = [];
    private readonly HashSet<Point> _toWake = [];
        
    public override FloatCell[,] InitWorld(int width, int height)
    {
        _world = new FloatCell[width, height];
        _newWorld = new FloatCell[width, height];

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
                _checkSet.Add(_world[x,y].Pos);
            }
        }
        
        return _world;
    }

    public override FloatCell[,] Iterate()
    {
        _toWake.Clear();
        _toSleep.Clear();
        
        for (int i = 0; i < _colors.Length; i++)
        {
            _colors[i] = Mutate(_colors[i], 5);
        }
        
        foreach ((int x, int y) in _checkSet)
        {
            FloatCell current = _world[x, y];
            _neighbors = current.GetMoore(_world, 1, true, _neighbors);

            double highest = double.MinValue;
            FloatCell winner = current;

            bool sleep = true;
            foreach (FloatCell neighbor in _neighbors)
            {
                if (neighbor.Strength > highest)
                {
                    highest = neighbor.Strength;
                    winner = neighbor;
                }
                if (neighbor.Id != current.Id)
                {
                    sleep = false;
                }
            }

            if (sleep)
            {
                _toSleep.Add(current.Pos);
                _newWorld[x, y] = new FloatCell(x, y, highest, winner.Id, current.Col);
            }
            else
            {
                foreach (FloatCell neighbor in _neighbors)
                {
                    _toWake.Add(neighbor.Pos);
                }
                
                if (winner.Id != current.Id)
                {
                    _newWorld[x, y] = new FloatCell(x, y, highest, winner.Id, _colors[winner.Id]);
                }
                else
                {
                    _newWorld[x, y] = new FloatCell(x, y, highest, winner.Id, current.Col);
                }
            }
            
            _newWorld[x, y].Strength += Rand.NextDouble() - 1;
        }
        
        _checkSet.ExceptWith(_toSleep);
        _checkSet.UnionWith(_toWake);
        
        Debug.WriteLine("Awake: " + _checkSet.Count);
        (_newWorld, _world) = (_world, _newWorld);
        return _world;
    }
}