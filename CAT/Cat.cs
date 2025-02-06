using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CommunityToolkit.HighPerformance;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CAT;

public class Cat : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _tex;
    private Color[] _backingColors;
    private Memory2D<Color> _colors;
    private bool _imageSaved;

    private Cell[,] _world;
    private const int WorldX = 300;
    private const int WorldY = 300;
    private int _iterations;

    private Iterator _iterator;
    private const int SpeedUp = 1;
    private Random _rand = new();
    private int _seed;

    public Cat()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = false;
    }

    protected override void Initialize()
    {
        _seed = Environment.TickCount;
        _rand = new Random(_seed);
        Iterator.Rand = _rand;
        _iterator = new StrengthMutation(2);

        _backingColors = new Color[WorldX * WorldY];
        _colors = new Memory2D<Color>(_backingColors, WorldX, WorldY);
        _tex = new Texture2D(GraphicsDevice, WorldX, WorldY);
        
        foreach (ref Color c in _backingColors.AsSpan())
            c = Color.Black;
        
        InactiveSleepTime = TimeSpan.Zero;
        _graphics.PreferredBackBufferWidth = 1000;
        _graphics.PreferredBackBufferHeight = 1000;
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();

        _world = _iterator.InitWorld(WorldX, WorldY);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_iterator.Completed)
        {
            if (!_imageSaved)
            {
                SaveImage();
                _imageSaved = true;
            }
        }
        else
        {
            _world = _iterator.Iterate();
            _iterations++;
            if (SpeedUp == 0 || _iterations % SpeedUp != 0)
            {
                SuppressDraw();
            }
            else
            {
                var c = _colors.Span;
                for (int x = 0; x < WorldX; x++)
                {
                    for (int y = 0; y < WorldY; y++)
                    {
                        c[x, y] = _world[x, y].Col;
                    }
                }
            }
        }

        double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
        Debug.WriteLine(
            "fps: " + (1000 / ms) + " (" + ms + "ms)" + " iterations: " + _iterations
        );

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);
        
        _tex.SetData(_backingColors);
        _spriteBatch.Draw(_tex, new Rectangle(0, 0, 1000, 1000), Color.White);
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
    
    private void SaveImage()
    {
        string date = DateTime.Now.ToString("s").Replace("T", " ").Replace(":", "-");
        Stream stream = new FileStream($"{date}_i{_iterations}.png", FileMode.Create);
        _tex.SaveAsPng(stream, _tex.Width, _tex.Height);
    }
}