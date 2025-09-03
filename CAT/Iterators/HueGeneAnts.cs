using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.HighPerformance;
using Microsoft.Xna.Framework;

namespace CAT;

public class HueGeneAnts : Iterator
{
    private FloatCell[,] _world;
    private FloatCell[,] _newWorld;
    private int _width;
    private int _height;

    private const int PlantMutation = 8;
    private const int AntMutation = 10;
    private const int EnMultiplier = 10;
    private const int DeathDivider = 100;
    private const int BirthThreshold = 10;
    private const double GrowthChance = 0.15;
    private const int EdibleRange = 50;
    
    public override FloatCell[,] InitWorld(int width, int height)
    {
        _world = new FloatCell[width, height];
        _newWorld = new FloatCell[width, height];
        _width = width;
        _height = height;
        Color startCol = new Color(Rand.Next(256), Rand.Next(256), Rand.Next(256));
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Rand.NextDouble() > 0.99)
                {
                    if (Rand.NextDouble() > 0.99)
                    {
                        _world[x, y] = new FloatCell(x, y, Rand.NextDouble(), 1, startCol);
                    }
                    else
                    {
                        _world[x, y] = new FloatCell(x, y, Rand.NextDouble() * 10, 2, startCol);
                    }
                }
                else
                {
                    _world[x, y] = new FloatCell(x, y, 0, 0, Color.Black);
                }
            }
        }

        return _world;
    }

    public override FloatCell[,] Iterate()
    {
        List<Point> empty = [];
        int emptyCount = 0;
        int plantCount = 0;
        _newWorld.AsSpan().Clear();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                FloatCell current = _world[x, y];
                
                if (current.Id == 0) // Empty
                {
                    emptyCount++;
                    _newWorld[x, y] ??= current;
                }
                else if (current.Id == 1) // Plant
                {
                    plantCount++;
                    _newWorld[x, y] ??= current;
                    if (Rand.NextDouble() < GrowthChance)
                    {
                        Point dir = Direction.Cardinals[Rand.Next(4)];
                        Point loc = new((current.Pos.X + dir.X + _width) % _width, (current.Pos.Y + dir.Y + _height) % _height);
                        FloatCell adj = _world[loc.X, loc.Y];
                    
                        if (adj.Id == 0)
                        { 
                            FloatCell placed = new(loc.X, loc.Y, Rand.NextDouble(), 1, Mutate(current.Col, PlantMutation)) 
                            {
                                LastUpdate = _world[loc.X, loc.Y].LastUpdate,
                                Updates = _world[loc.X, loc.Y].Updates + 1,
                            };
                            
                            _newWorld[loc.X, loc.Y] = placed;
                        }
                    }

                }
                else if (current.Id == 2) // Ant
                {
                    Point moveTo = current.Pos;
                    bool eat = false;
                    empty.Clear();
                    
                    foreach (Point dir in Direction.Cardinals)
                    {
                        Point loc = new((current.Pos.X + dir.X + _width) % _width, (current.Pos.Y + dir.Y + _height) % _height);
                        FloatCell adj = _world[loc.X, loc.Y];
                        
                        if (adj.Id == 1)
                        {
                            if (Edible(current, adj))
                            {
                                moveTo = loc;
                                eat = true;
                            }
                        }
                        else if (adj.Id == 0)
                        {
                            empty.Add(loc);
                        }
                    }

                    if (eat)
                    {
                        _newWorld[x, y] = new FloatCell(x, y, 0, 0, Color.Black)
                        {
                            Updates = current.Updates + 1,
                            LastUpdate = Cat.Iterations
                        };

                        current.Strength += _world[moveTo.X, moveTo.Y].Strength * EnMultiplier;
                        current.Updates = _world[moveTo.X, moveTo.Y].Updates + 1;
                        current.LastUpdate = Cat.Iterations;
                        current.Pos = moveTo;
                        _newWorld[moveTo.X, moveTo.Y] = current;
                    }
                    else if (empty.Count > 0)
                    {
                        _newWorld[x, y] = new FloatCell(x, y, 0, 0, Color.Black)
                        {
                            Updates = current.Updates + 1,
                            LastUpdate = Cat.Iterations
                        };

                        moveTo = empty[Rand.Next(empty.Count)];
                        current.Updates = _world[moveTo.X, moveTo.Y].Updates + 1;
                        current.LastUpdate = Cat.Iterations;
                        current.Pos = moveTo;
                        _newWorld[moveTo.X, moveTo.Y] = current;
                    }
                    else
                    {
                        moveTo = current.Pos;
                        _newWorld[x, y] = current;
                    }

                    if (current.Strength > BirthThreshold && empty.Count > 0)
                    {
                        _newWorld[x, y] = new FloatCell(x, y, Rand.NextDouble() * 10, 2, Mutate(current.Col, AntMutation));
                    }
                    
                    current.Strength -= Rand.NextDouble()/DeathDivider;
                    if (current.Strength < 0)
                    {
                        _newWorld[moveTo.X, moveTo.Y] = new FloatCell(x, y, 0, 0, Color.Black)
                        {
                            Updates = current.Updates + 1,
                            LastUpdate = Cat.Iterations
                        };
                    }
                }
            }
        }

        if (emptyCount == 0 || plantCount == 0)
        {
            Completed = true;
        }
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }

    private bool Edible(FloatCell ant, FloatCell plant)
    {
        double diff = Math.Abs(ant.Col.R - plant.Col.R) + Math.Abs(ant.Col.G - plant.Col.G) + Math.Abs(ant.Col.B - plant.Col.B);
        return diff < EdibleRange;
    }
}