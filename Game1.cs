using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.World;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.Systems;
using KingdomOfDarkness.UI;
using KingdomOfDarkness.Data;

namespace KingdomOfDarkness;

public class Game1 : Game
{
    // State
    private GameStateType _currentState = GameStateType.ClassSelect;
    private ClassSelectScreen _classSelectScreen;
    private CompanionInfoPanel _companionInfoPanel;

    // Graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private DisplayScaler _displayScaler;
    private RenderTarget2D _virtualRenderTarget;
    private RenderTarget2D _uiRenderTarget;

    // Core Helpers
    private InputManager _inputManager;
    private Camera2D _camera;

    // Maps
    private MapType _currentMapType;
    private IsoTileMap _townMap;
    private CollisionMap _townCollisionMap;
    private IsoTileMap _huntingMap;
    private CollisionMap _huntingCollisionMap;
    
    private IsoTileMap _currentTileMap;
    private CollisionMap _currentCollisionMap;

    // Portals
    private List<Portal> _portals;

    // Entities
    private Texture2D _whitePixel;
    private Player _player;
    private List<Companion> _companions;
    private List<Monster> _monsters;
    private List<Entity> _entities;
    
    // Systems
    private IsoMovementSystem _movementSystem;
    private CompanionAISystem _companionAISystem;
    private MonsterAISystem _monsterAISystem;
    private MonsterSpawnSystem _monsterSpawnSystem;
    private CombatSystem _combatSystem;
    private LevelSystem _levelSystem;
    private DialogueReactionSystem _dialogueReactionSystem;
    private RenderOrderSystem _renderOrderSystem;

    // UI
    private Hud _hud;
    private DamagePopupManager _damagePopupManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // Set Default Windowed mode
        _graphics.PreferredBackBufferWidth = GameConstants.DefaultWindowWidth;
        _graphics.PreferredBackBufferHeight = GameConstants.DefaultWindowHeight;
        _graphics.IsFullScreen = false;
        _graphics.HardwareModeSwitch = false;
        Window.AllowUserResizing = true;
        _graphics.ApplyChanges();

        _displayScaler = new DisplayScaler(GameConstants.VirtualWidth, GameConstants.VirtualHeight);

        _inputManager = new InputManager(_displayScaler);
        _camera = new Camera2D(GameConstants.VirtualWidth, GameConstants.VirtualHeight);
        
        _movementSystem = new IsoMovementSystem();
        _companionAISystem = new CompanionAISystem();
        _monsterAISystem = new MonsterAISystem();
        _levelSystem = new LevelSystem();
        _damagePopupManager = new DamagePopupManager();
        _combatSystem = new CombatSystem(_levelSystem, _damagePopupManager);
        _dialogueReactionSystem = new DialogueReactionSystem();
        _renderOrderSystem = new RenderOrderSystem();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        _virtualRenderTarget = new RenderTarget2D(
            GraphicsDevice,
            GameConstants.VirtualWidth,
            GameConstants.VirtualHeight,
            false,
            GraphicsDevice.PresentationParameters.BackBufferFormat,
            DepthFormat.Depth24);

        _uiRenderTarget = new RenderTarget2D(
            GraphicsDevice,
            GameConstants.VirtualWidth,
            GameConstants.VirtualHeight,
            false,
            GraphicsDevice.PresentationParameters.BackBufferFormat,
            DepthFormat.Depth24);

        // Load Korean-capable SpriteFont
        FontManager.LoadContent(Content);

        // Generate white 1x1 texture for simple primitives
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
        
        KingdomOfDarkness.UI.UIHelper.Initialize(_whitePixel);

        // Initialize Data
        ClassDatabase.Initialize();

        // Instantiate maps
        _townMap = new IsoTileMap(GraphicsDevice, 30, 30);
        _townCollisionMap = new CollisionMap(_townMap);

        _huntingMap = new IsoTileMap(GraphicsDevice, 50, 50);
        _huntingCollisionMap = new CollisionMap(_huntingMap);

        // Instantiate UI
        _hud = new Hud(_whitePixel);
        _classSelectScreen = new ClassSelectScreen(_whitePixel);
        _companionInfoPanel = new CompanionInfoPanel(_whitePixel);
    }

    private void StartGame(CharacterClassType selectedClass)
    {
        // Instantiate player at map center with selected class
        _player = new Player(_whitePixel, new Vector2(10f, 10f), selectedClass);

        // Instantiate companions in town
        _companions = new List<Companion>
        {
            new Companion(_whitePixel, new Vector2(12f, 10f), "아리아", CharacterClassType.Priest),
            new Companion(_whitePixel, new Vector2(10f, 12f), "엘리스", CharacterClassType.Mage),
            new Companion(_whitePixel, new Vector2(12f, 12f), "카일", CharacterClassType.Rogue)
        };

        foreach (var c in _companions) c.IsRecruited = false;

        // Create Portals
        _portals = new List<Portal>
        {
            // Town -> Hunting Ground (Portal located at Town 25, 25)
            new Portal(_whitePixel, new Vector2(25f, 25f), MapType.HuntingGround, new Vector2(5f, 5f), Color.Blue),
            // Hunting Ground -> Town (Portal located at Hunting Ground 4, 4)
            new Portal(_whitePixel, new Vector2(4f, 4f), MapType.Town, new Vector2(24f, 25f), Color.Green)
        };

        // Initialize Monster Spawn System (only for Hunting Ground)
        _monsters = new List<Monster>();
        _monsterSpawnSystem = new MonsterSpawnSystem(_whitePixel, _huntingMap);
        _monsterSpawnSystem.PopulateMap(_monsters, new List<Entity>()); // We'll pass recruited entities dynamically

        // Set initial map
        SwitchMap(MapType.Town, _player.WorldPosition);

        _currentState = GameStateType.Playing;
    }

    private void SwitchMap(MapType mapType, Vector2 spawnPosition)
    {
        _currentMapType = mapType;
        _player.WorldPosition = spawnPosition;
        _camera.LookAt(_player.WorldPosition);

        // Teleport recruited companions around player
        var recruitedCompanions = new List<Companion>();
        foreach (var c in _companions)
        {
            if (c.IsRecruited)
            {
                c.WorldPosition = spawnPosition + new Vector2(-1f, 1f); // Simple offset
                recruitedCompanions.Add(c);
            }
        }

        // Set active map references
        if (mapType == MapType.Town)
        {
            _currentTileMap = _townMap;
            _currentCollisionMap = _townCollisionMap;
        }
        else
        {
            _currentTileMap = _huntingMap;
            _currentCollisionMap = _huntingCollisionMap;
        }

        // Rebuild _entities list for the current map
        _entities = new List<Entity> { _player };
        
        // Add companions (in Town, show all; in Hunting, show only recruited)
        foreach (var c in _companions)
        {
            if (mapType == MapType.Town || c.IsRecruited)
            {
                _entities.Add(c);
            }
        }

        // Add portals for this map
        foreach (var p in _portals)
        {
            // If we are in Town, add portals targeting HuntingGround, etc.
            if ((mapType == MapType.Town && p.TargetMapType == MapType.HuntingGround) ||
                (mapType == MapType.HuntingGround && p.TargetMapType == MapType.Town))
            {
                _entities.Add(p);
            }
        }

        // Add monsters if in hunting ground
        if (mapType == MapType.HuntingGround)
        {
            foreach (var m in _monsters) _entities.Add(m);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboardState = Keyboard.GetState();

        // Handle exit input
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            keyboardState.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        // Toggle Fullscreen on Alt+Enter or F11
        if ((keyboardState.IsKeyDown(Keys.Enter) && (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))) || 
            keyboardState.IsKeyDown(Keys.F11))
        {
            // Simple debounce using previous keyboard state handled inside InputManager, but we are before InputManager.Update.
            // Let's just use InputManager.IsKeyPressed for F11 or Alt+Enter.
        }

        // Update inputs
        _inputManager.Update();

        if ((_inputManager.IsKeyPressed(Keys.Enter) && (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt))) || 
            _inputManager.IsKeyPressed(Keys.F11))
        {
            _graphics.ToggleFullScreen();
        }

        if (_currentState == GameStateType.ClassSelect)
        {
            var selectedClass = _classSelectScreen.Update(_inputManager);
            if (selectedClass.HasValue)
            {
                StartGame(selectedClass.Value);
            }
            return;
        }

        if (_currentState != GameStateType.Playing) return;

        // --- MOUSE PICKING (Recruitment) ---
        // 1. If companion info panel is open, update it and skip other interactions
        if (_companionInfoPanel.Update(_inputManager))
        {
            // Handled a click inside the panel (e.g. recruit accepted)
        }
        else if (_inputManager.IsLeftMouseClicked())
        {
            // 2. Check if player clicked on an unrecruited companion
            Vector2 mouseWorldPos = _camera.ScreenToWorld(_inputManager.MousePosition);
            
            foreach (var companion in _companions)
            {
                if (!companion.IsRecruited && !companion.IsDead)
                {
                    // Basic distance check (picking radius ~ 1.5 world units)
                    float dist = Vector2.Distance(mouseWorldPos, companion.WorldPosition);
                    if (dist < 1.5f)
                    {
                        _companionInfoPanel.Open(companion);
                        break;
                    }
                }
            }
        }

        // --- FILTER RECRUITED COMPANIONS ---
        var recruitedCompanions = new List<Companion>();
        foreach (var c in _companions)
        {
            if (c.IsRecruited) recruitedCompanions.Add(c);
        }

        // Update Player Input and Physics
        _player.UpdateInput(_inputManager);
        _movementSystem.Update(gameTime, _player, _currentCollisionMap, pos => IsTileOccupied(pos, _player));
        _player.Update(gameTime);

        // Check Portal collision
        foreach (var p in _portals)
        {
            // Only check portals active in current map
            if ((_currentMapType == MapType.Town && p.TargetMapType == MapType.HuntingGround) ||
                (_currentMapType == MapType.HuntingGround && p.TargetMapType == MapType.Town))
            {
                if (Vector2.Distance(_player.WorldPosition, p.WorldPosition) < 1.0f)
                {
                    SwitchMap(p.TargetMapType, p.TargetSpawnPosition);
                    return; // Stop update for this frame since map changed
                }
            }
        }

        // Update Dialogue reaction system cooldowns
        _dialogueReactionSystem.Update(gameTime);

        // Update Companion AI, Speech Bubble, and Physics
        foreach (var companion in _companions)
        {
            if (_currentMapType == MapType.HuntingGround && !companion.IsRecruited) continue;

            _companionAISystem.Update(gameTime, companion, _player, _dialogueReactionSystem);
            // Only update physics and bubble if recruited, else they just stand there
            if (companion.IsRecruited)
            {
                _movementSystem.Update(gameTime, companion, _currentCollisionMap, pos => IsTileOccupied(pos, companion));
            }
            companion.Update(gameTime);
            companion.Bubble.Update(gameTime);
        }

        // Update Monsters (only in Hunting Ground)
        if (_currentMapType == MapType.HuntingGround)
        {
            _monsterSpawnSystem.Update(gameTime, _monsters);

            foreach (var monster in _monsters)
            {
                _monsterAISystem.Update(gameTime, monster, _player, recruitedCompanions);
                _movementSystem.Update(gameTime, monster, _currentCollisionMap, pos => IsTileOccupied(pos, monster));
                monster.Update(gameTime);

                monster.IsTargeted = (_player.Target == monster);
            }

            _combatSystem.Update(gameTime, _player, recruitedCompanions, _monsters, _inputManager.IsAttackRequested, _dialogueReactionSystem);
            _damagePopupManager.Update(gameTime);
        }

        // Camera follow player in world-screen space
        Vector2 targetScreenPos = IsoMath.WorldToScreen(_player.WorldPosition);
        _camera.FollowScreenPosition(targetScreenPos, 0.1f);

        base.Update(gameTime);
    }

    private bool IsTileOccupied(Vector2 pos, Character ignoreChar)
    {
        if (_player != ignoreChar && !_player.IsDead && _player.WorldPosition == pos) return true;
        foreach (var c in _companions)
        {
            if (c != ignoreChar && !c.IsDead && c.WorldPosition == pos) return true;
        }
        if (_currentMapType == MapType.HuntingGround)
        {
            foreach (var m in _monsters)
            {
                if (m != ignoreChar && !m.IsDead && m.WorldPosition == pos) return true;
            }
        }
        return false;
    }

    protected override void Draw(GameTime gameTime)
    {
        _displayScaler.Update(
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight);

        // 1. Draw game into virtual 4:3 canvas (Pixel Art)
        GraphicsDevice.SetRenderTarget(_virtualRenderTarget);
        GraphicsDevice.Clear(new Color(20, 24, 28)); // Sleek dark gray background

        if (_currentState == GameStateType.Playing)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            
            // Draw isometric map
            _currentTileMap.Draw(_spriteBatch, _camera);

            // Draw entities in depth-sorted order
            _renderOrderSystem.DrawEntities(_spriteBatch, _camera, _entities);

            _spriteBatch.End();
        }

        // 2. Draw UI into UI 4:3 canvas (Smooth Text)
        GraphicsDevice.SetRenderTarget(_uiRenderTarget);
        GraphicsDevice.Clear(Color.Transparent);

        if (_currentState == GameStateType.ClassSelect)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
            _classSelectScreen.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        else if (_currentState == GameStateType.Playing)
        {
            // Draw Speech Bubbles, Damage Popups, and Entity Labels (LinearClamp for text)
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);

            foreach (var entity in _entities)
            {
                if (entity is Character character && character.IsActive)
                {
                    character.DrawUI(_spriteBatch, _camera);
                }
            }

            foreach (var companion in _companions)
            {
                if (_currentMapType == MapType.HuntingGround && !companion.IsRecruited) continue;
                companion.Bubble.Draw(_spriteBatch, _camera, companion, _whitePixel);
            }

            if (_currentMapType == MapType.HuntingGround)
            {
                _damagePopupManager.Draw(_spriteBatch, _camera);
            }

            _spriteBatch.End();

            // UI Layer (LinearClamp for text)
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
            // Draw HUD overlay on top of everything
            _hud.Draw(_spriteBatch, _player, _companions, _whitePixel);

            // Draw Companion Info Panel if open
            _companionInfoPanel.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        // 3. Draw final canvases into actual backbuffer
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.Black);

        // Draw pixel art canvas with PointClamp
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone);

        _spriteBatch.Draw(
            _virtualRenderTarget,
            _displayScaler.DestinationRectangle,
            Color.White);

        _spriteBatch.End();

        // Draw UI canvas with LinearClamp
        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.LinearClamp,
            DepthStencilState.None,
            RasterizerState.CullNone);

        _spriteBatch.Draw(
            _uiRenderTarget,
            _displayScaler.DestinationRectangle,
            Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
