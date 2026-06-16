using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.World;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.Systems;

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

    // Entities
    private Texture2D _whitePixel;
    private Player _player;
    private Companion _companion;
    private List<Monster> _monsters;
    private List<Entity> _entities;
    
    // Systems
    private IsoMovementSystem _movementSystem;
    private CompanionAISystem _companionAISystem;
    private MonsterAISystem _monsterAISystem;
    private CombatSystem _combatSystem;
    private LevelSystem _levelSystem;
    private RenderOrderSystem _renderOrderSystem;

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

        // Instantiate player at map center
        _player = new Player(_whitePixel, new Vector2(10f, 10f));

        // Instantiate companion near player
        _companion = new Companion(_whitePixel, _player.WorldPosition + new Vector2(-1f, 1f));

        // Instantiate monsters
        _monsters = new List<Monster>
        {
            new Monster(_whitePixel, new Vector2(12f, 8f))
        };

        // Instantiate entity list and add characters
        _entities = new List<Entity> { _player, _companion };
        foreach (var monster in _monsters)
        {
            _entities.Add(monster);
        }

        // Instantiate systems
        _movementSystem = new IsoMovementSystem();
        _companionAISystem = new CompanionAISystem();
        _monsterAISystem = new MonsterAISystem();
        _levelSystem = new LevelSystem();
        _combatSystem = new CombatSystem(_levelSystem);
        _renderOrderSystem = new RenderOrderSystem();

        // Position camera to look at the player immediately
        _camera.LookAt(_player.WorldPosition);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Generate white 1x1 texture for simple primitives
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
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

        // Update Player Input and Physics
        _player.UpdateInput(_inputManager);
        _movementSystem.Update(gameTime, _player);
        _player.Update(gameTime);

        // Update Companion AI and Physics
        _companionAISystem.Update(gameTime, _companion, _player);
        _movementSystem.Update(gameTime, _companion);
        _companion.Update(gameTime);

        // Update Monsters AI and Physics
        foreach (var monster in _monsters)
        {
            _monsterAISystem.Update(gameTime, monster, _player, _companion);
            _movementSystem.Update(gameTime, monster);
            monster.Update(gameTime);
        }

        // Update Combat interactions (handles cooldowns, damage, and rewards)
        _combatSystem.Update(gameTime, _player, _companion, _monsters, _inputManager.IsAttackRequested);

        // Camera follow player in world-screen space
        Vector2 targetScreenPos = IsoMath.WorldToScreen(_player.WorldPosition);
        _camera.FollowScreenPosition(targetScreenPos, 0.1f);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(20, 24, 28)); // Sleek dark gray background

        _spriteBatch.Begin();
        
        // Draw isometric map
        _tileMap.Draw(_spriteBatch, _camera);

        // Draw entities in depth-sorted order
        _renderOrderSystem.DrawEntities(_spriteBatch, _camera, _entities);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

