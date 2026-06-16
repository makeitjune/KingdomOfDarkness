using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.World;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.Systems;
using KingdomOfDarkness.UI;

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
    private CollisionMap _collisionMap;

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
    private DialogueReactionSystem _dialogueReactionSystem;
    private RenderOrderSystem _renderOrderSystem;

    // UI
    private Hud _hud;

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
        // Instantiate core logical systems
        _inputManager = new InputManager();
        _camera = new Camera2D(GameConstants.ScreenWidth, GameConstants.ScreenHeight);
        _movementSystem = new IsoMovementSystem();
        _companionAISystem = new CompanionAISystem();
        _monsterAISystem = new MonsterAISystem();
        _levelSystem = new LevelSystem();
        _combatSystem = new CombatSystem(_levelSystem);
        _dialogueReactionSystem = new DialogueReactionSystem();
        _renderOrderSystem = new RenderOrderSystem();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Generate white 1x1 texture for simple primitives
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });

        // Instantiate map after GraphicsDevice is initialized
        _tileMap = new IsoTileMap(GraphicsDevice, 20, 20);

        // Instantiate collision map
        _collisionMap = new CollisionMap(_tileMap);

        // Instantiate player at map center
        _player = new Player(_whitePixel, new Vector2(10f, 10f));

        // Instantiate companion near player
        _companion = new Companion(_whitePixel, _player.WorldPosition + new Vector2(-1f, 1f));

        // Instantiate monsters (placed closer to player so they are visible on screen immediately)
        _monsters = new List<Monster>
        {
            new Monster(_whitePixel, new Vector2(11.5f, 9.5f))
        };

        // Instantiate entity list and add characters
        _entities = new List<Entity> { _player, _companion };
        foreach (var monster in _monsters)
        {
            _entities.Add(monster);
        }

        // Instantiate UI
        _hud = new Hud(_whitePixel);

        // Position camera to look at the player immediately
        _camera.LookAt(_player.WorldPosition);
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
        _movementSystem.Update(gameTime, _player, _collisionMap);
        _player.Update(gameTime);

        // Update Dialogue reaction system cooldowns
        _dialogueReactionSystem.Update(gameTime);

        // Update Companion AI, Speech Bubble, and Physics
        _companionAISystem.Update(gameTime, _companion, _player, _dialogueReactionSystem);
        _movementSystem.Update(gameTime, _companion, _collisionMap);
        _companion.Update(gameTime);
        _companion.Bubble.Update(gameTime);

        // Update Monsters AI and Physics
        foreach (var monster in _monsters)
        {
            _monsterAISystem.Update(gameTime, monster, _player, _companion);
            _movementSystem.Update(gameTime, monster, _collisionMap);
            monster.Update(gameTime);

            // Update targeted status
            monster.IsTargeted = (_player.Target == monster);
        }

        // Update Combat interactions (handles cooldowns, damage, and rewards)
        _combatSystem.Update(gameTime, _player, _companion, _monsters, _inputManager.IsAttackRequested, _dialogueReactionSystem);

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

        // Draw speech bubbles above characters (after entities are drawn)
        _companion.Bubble.Draw(_spriteBatch, _camera, _companion, _whitePixel);

        // Draw HUD overlay on top of everything
        _hud.Draw(_spriteBatch, _player, _companion, _whitePixel);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

