using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CAT;

public class ChessSheep : Iterator
{
    private List<IntCell> _neighbors = [];
    private IntCell[,] _world;
    private IntCell[,] _newWorld;
    private Color[] _cols = [Color.White, Color.Black, Color.Orange, Color.Violet, Color.Red, Color.Yellow];
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
                int type = Rand.Next(6);
                _world[x, y] = new IntCell(x, y, type, _cols[type]);
            }
        }
            
        return _world;
    }

    public override IntCell[,] Iterate()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                IntCell current = _world[x, y];
                _neighbors = current.GetChess(_world, current.Strength, _neighbors);
                int[] counts = new int[6];

                int highest = 0;
                int index = -1;
                
                foreach (IntCell neighbor in _neighbors)
                {
                    if (neighbor != null)
                    {
                        counts[neighbor.Strength]++;
                    
                        if (counts[neighbor.Strength] > highest || counts[neighbor.Strength] == highest && Rand.Next(2) == 0)
                        {
                            highest = counts[neighbor.Strength];
                            index = neighbor.Strength;
                        }
                    }
                }

                if (index == current.Strength)
                {
                    _newWorld[x, y] = current;
                }
                else
                {
                    _newWorld[x, y] = new IntCell(x, y, index, _cols[index]);
                    _newWorld[x,y].Updates = current.Updates + 1;
                    _newWorld[x, y].LastUpdate = Cat.Iterations;
                }
            }
        }
        
        (_world, _newWorld) = (_newWorld, _world);
        return _world;
    }
}