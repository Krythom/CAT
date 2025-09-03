using System;
using Microsoft.Xna.Framework;

namespace CAT;

public class Gem : Iterator
{
    private DirectionalCell[,] _world;
    private DirectionalCell _miner;
    private readonly DirectionalCell _empty = new(0, 0, Color.Black);
    private const int Move = 1;
    private int _turn = 1;
    private int _width;
    private int _height;
    
    public override DirectionalCell[,] InitWorld(int width, int height)
    {
        _world = new DirectionalCell[height, width];
        _width = width;
        _height = height;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _world[x, y] = _empty;
            }
        }
        
        _miner = new DirectionalCell(width/2, height/2, 0, Color.White);
        _world[width / 2, height / 2] = _miner;
        
        return _world;
    }

    public override DirectionalCell[,] Iterate()
    {
        int dist = Move;
        while (_miner.GetCell(_world, _miner.Dir.X * dist, _miner.Dir.Y * dist, true).Index >= 0)
        {
            dist++;
            _turn = (_turn += 1) % 4;

            if (_miner.GetCell(_world, _miner.Dir.X * dist, _miner.Dir.Y * dist, true) == _miner)
            {
                Completed = true;
                break;
            }
        }
        
        
        _world[_miner.Pos.X, _miner.Pos.Y] = new DirectionalCell(_miner.Pos.X, _miner.Pos.Y, _miner.Index, _miner.Col);
        
        _miner.Pos.X = (_width + _miner.Pos.X + _miner.Dir.X * dist) % _width;
        _miner.Pos.Y = (_height + _miner.Pos.Y + _miner.Dir.Y * dist) % _height;
        _miner.Index = (_miner.Index + _turn + 4) % 4;
        _miner.Dir = Direction.Cardinals[_miner.Index];
        _miner.Col = Mutate(_miner.Col, 5);
        
        _world[_miner.Pos.X, _miner.Pos.Y] = _miner;
        
        return _world;
    }
}