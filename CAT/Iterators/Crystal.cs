using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CAT;

public class Crystal : Iterator
{
    private const double Rarity = 0.00001;
    private const int Num = 5;
    private readonly HashSet<Point> _toBond = [];
    private List<Molecule> _neighbors;
    private Molecule[,] _world;
    private Molecule[,] _newWorld;
    private int _width;
    private int _height;

    private readonly List<float[]> _bondSets = [];
    private readonly List<Color> _colors = [];
    
    public override Molecule[,] InitWorld(int width, int height)
    {
        _width = width;
        _height = height;
        _world = new Molecule[_width, _height];
        _newWorld = new Molecule[_width, _height];

        for (int i = 0; i < Num; i++)
        {
            _bondSets.Add([Rand.NextSingle()*2 - 1, Rand.NextSingle()*2 - 1, Rand.NextSingle()*2 - 1, Rand.NextSingle()*2 - 1]);
            _colors.Add(new Color(Rand.Next(256), Rand.Next(256), Rand.Next(256)));
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (Rand.NextDouble() < Rarity)
                {
                    int num = Rand.Next(_bondSets.Count);
                    float[] bonds = _bondSets[num];
                    
                    Molecule placed = new(x, y, bonds[0], bonds[1], bonds[2], bonds[3], _colors[num])
                    {
                        Locked = true
                    };

                    _toBond.Add(new Point((x + 1) % width, y));
                    _toBond.Add(new Point((x - 1 + _width) % width, y));
                    _toBond.Add(new Point(x, (y + 1) % _height));
                    _toBond.Add(new Point(x, (y - 1 + _height) % _height));
                    _world[x, y] = placed;
                }
                else
                {
                    _world[x, y] = new Molecule(x,y,0.1f,0.1f,0.1f,0.1f, Color.Black);
                }
            }
        }
        
        return _world;
    }

    public override Molecule[,] Iterate()
    {
        HashSet<Point> toAdd = [];

        if (_toBond.Count > 0)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (_toBond.Contains(new Point(x, y)))
                    {
                        Molecule placed = _world[x, y];
                        _toBond.Remove(new Point(x, y));
                    
                        float max = float.MinValue;
                        int index = 0;

                        for (int i = 0; i < _bondSets.Count; i++) 
                        {
                            float[] bonds = _bondSets[i];
                            float stability = GetStab(placed, bonds);

                            if (stability > max)
                            {
                                max = stability;
                                index = i;
                            }
                        }
                    
                        float[] best = _bondSets[index];
                        placed = new Molecule(x, y, best[0], best[1], best[2], best[3], _colors[index])
                        {
                            Locked = true,
                            LastUpdate = Cat.Iterations
                        };
                        _newWorld[x, y] = placed;

                        foreach (Point p in Direction.Cardinals)
                        {
                            Molecule next = placed.GetCell(_world, p.X, p.Y, true);
                            if (!next.Locked && Rand.NextDouble() < 0.495)
                            {
                                toAdd.Add(next.Pos);
                            }
                        }
                    }
                    else
                    {
                        _newWorld[x, y] = _world[x, y];
                    }
                }
            }
        }
        else
        {
            Completed = true;
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Molecule mol = _world[x, y];
                    float stab = GetStab(mol, [mol.Up, mol.Right, mol.Down, mol.Left]);
                    mol.Updates = (int)(Math.Abs(stab) * 10000);
                }
            }
        }

        _toBond.UnionWith(toAdd);
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }

    private float GetStab(Molecule mol, float[] bonds)
    {
        float stability = 0;
                
        stability += mol.GetCell(_world,0,-1,true).Down * bonds[0];
        stability += mol.GetCell(_world,1,0,true).Left * bonds[1];
        stability += mol.GetCell(_world,0,1,true).Up * bonds[2];
        stability += mol.GetCell(_world,-1,0,true).Right * bonds[3];

        return stability;
    }
}