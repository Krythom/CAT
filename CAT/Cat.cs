using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CommunityToolkit.HighPerformance;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CAT;

public class Cat : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _tex;
    private Color[] _backingColors;
    private Memory2D<Color> _colors;
    private bool _saved;

    private Cell[,] _world;
    private const int WorldX = 700;
    private const int WorldY = 700;
    public static int Iterations;

    private HueGeneAnts _iterator;
    private const int SpeedUp = 1;
    private Random _rand = new();
    private int _seed;

    private bool _paused = true;

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
        _iterator = new HueGeneAnts();

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
        Input.Update();
        
        if (Input.KeyPressed(Keys.Space))
        {
            _paused = !_paused;
        }

        if (Input.KeyPressed(Keys.R))
        {
            Initialize();
            _saved = false;
        }
        
        if (_iterator.Completed || Input.KeyPressed(Keys.Escape))
        {
            if (!_saved)
            {
                SaveImage();
                JsonCreation.CreateJson(_world, WorldX, WorldY, Iterations);
                _saved = true;
            }
            Initialize();
            _saved = false;
        }
        else
        {
            if (!_paused || Input.KeyPressed(Keys.OemPeriod))
            {
                _world = _iterator.Iterate();
                Iterations++;
                if (SpeedUp == 0 || Iterations % SpeedUp != 0)
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
        }

        if (Input.KeyPressed(Keys.Escape))
        {
            Exit();
        }

        double ms = gameTime.ElapsedGameTime.TotalMilliseconds;
        
        Debug.WriteLine(
            "fps: " + (1000 / ms) + " (" + ms + "ms)" + " iterations: " + Iterations
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
        Stream stream = new FileStream($"{date}_i{Iterations}.png", FileMode.Create);
        _tex.SaveAsPng(stream, _tex.Width, _tex.Height);
    }
}