using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.World;

namespace KingdomOfDarkness;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Core Helpers
    private InputManager _inputManager;
    private Camera2D _camera;

    // World Map
    private IsoTileMap _tileMap;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Set screen resolution from constants
        _graphics.PreferredBackBufferWidth = GameConstants.ScreenWidth;
        _graphics.PreferredBackBufferHeight = GameConstants.ScreenHeight;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // Instantiate core systems
        _inputManager = new InputManager();
        _camera = new Camera2D(GameConstants.ScreenWidth, GameConstants.ScreenHeight);

        base.Initialize();

        // Instantiate map after GraphicsDevice is initialized
        _tileMap = new IsoTileMap(GraphicsDevice, 20, 20);

        // Position camera to look at the center of the 20x20 map
        _camera.LookAt(new Vector2(10f, 10f));
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        // Handle exit input
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // Update inputs
        _inputManager.Update();

        // Temporary Camera Movement for Map Inspection in Phase 2
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_inputManager.MovementIntent != Vector2.Zero)
        {
            Vector2 screenDir = IsoMath.WorldToScreen(_inputManager.MovementIntent);
            if (screenDir.LengthSquared() > 0f)
            {
                screenDir.Normalize();
                _camera.Position += screenDir * 500f * dt;
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 24, 28)); // Sleek dark gray background

        _spriteBatch.Begin();
        _tileMap.Draw(_spriteBatch, _camera);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

