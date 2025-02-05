using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace CAT;

public class Soldiers(int types) : Iterator
{
    private Soldier[,] _world;
    private int _width;
    private int _height;
    private Color[] _colors;
    private List<Point> _indices = [];
    
    public override Soldier[,] InitWorld(int width, int height)
    {
        _world = new Soldier[width, height];
        _colors = new Color[types];
        _width = width;
        _height = height;

        for (int i = 0; i < types; i++)
        {
            _colors[i] = new Color(Rand.Next(255), Rand.Next(255), Rand.Next(255));
        }
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int id = Rand.Next(types);
                _world[x, y] = new Soldier(x, y, id, 1, _colors[id]);
                _indices.Add(new Point(x, y));
            }
        }
        
        _indices = _indices.OrderBy(x => Rand.Next()).ToList();
        return _world;
    }

    public override Soldier[,] Iterate()
    {
        for (int i = 0; i < _colors.Length; i++)
        {
            _colors[i] = Mutate(_colors[i], 1);
        }
        
        List<Soldier> neighbors = [];
        foreach (Point p in _indices)
        {
            Soldier current = _world[p.X, p.Y];
            
            neighbors = current.GetMoore(_world, 1, true, neighbors);
            List<Soldier> allies = [];
            List<Soldier> enemies = [];

            foreach (Soldier neighbor in neighbors)
            {
                if (neighbor.Id == current.Id)
                {
                    allies.Add(neighbor);
                }
                else
                {
                    enemies.Add(neighbor);
                }
            }

            // Build
            if (allies.Count == 8)
            {
                current.Count++;
            }

            // Move
            double total = current.Count;
            foreach (Soldier ally in allies)
            {
               total += ally.Count;
            }
            
            total /= allies.Count + 1;
            current.Count = total;
            
            foreach (Soldier ally in allies)
            {
                ally.Count = total;
            }

            // Fight
            double enemyStrength = 0;
            foreach (Soldier enemy in enemies)
            {
                enemyStrength += enemy.Count;
            }

            double diff = current.Count - enemyStrength;
            diff /= enemies.Count + 1;

            if (diff < 0)
            {
                Soldier winner = enemies[Rand.Next(enemies.Count)];
                current.Col = _colors[current.Id];
                current.Id = winner.Id;
                current.Count = -diff;
                
                foreach (Soldier enemy in enemies)
                {
                    enemy.Count = -diff;
                }
            }
            else
            {
                current.Count = diff;
                
                foreach (Soldier enemy in enemies)
                {
                    enemy.Id = current.Id;
                    enemy.Col = Mutate(current.Col, 10);
                    enemy.Count = diff;
                }
            }
        }

        return _world;
    }
}