using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KingdomOfDarkness.Core;

namespace KingdomOfDarkness;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    // Core Helpers
    private InputManager _inputManager;
    private Camera2D _camera;

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

        // Update camera (can follow Vector2.Zero for now)
        _camera.FollowScreenPosition(Vector2.Zero, 0.1f);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        base.Draw(gameTime);
    }
}

