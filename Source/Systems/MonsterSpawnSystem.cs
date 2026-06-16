using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Entities;
using KingdomOfDarkness.Data;
using KingdomOfDarkness.World;

namespace KingdomOfDarkness.Systems;

/// <summary>
/// Manages spawning, respawning, and placing of multiple monsters in the world.
/// </summary>
public class MonsterSpawnSystem
{
    private readonly Random _rnd = new Random();
    private readonly Texture2D _whitePixel;
    private readonly IsoTileMap _tileMap;

    public MonsterSpawnSystem(Texture2D whitePixel, IsoTileMap tileMap)
    {
        _whitePixel = whitePixel;
        _tileMap = tileMap;
        MonsterDatabase.Initialize();
    }

    /// <summary>
    /// Initially populate the map with some monsters.
    /// </summary>
    public void PopulateMap(List<Monster> monsters, List<Entity> allEntities)
    {
        // Add a mix of different monsters
        SpawnMonster("Goblin", new Vector2(10, 10), monsters, allEntities);
        SpawnMonster("Goblin", new Vector2(12, 11), monsters, allEntities);
        SpawnMonster("Skeleton", new Vector2(15, 8), monsters, allEntities);
        SpawnMonster("Skeleton", new Vector2(16, 9), monsters, allEntities);
        SpawnMonster("Wolf", new Vector2(5, 14), monsters, allEntities);
        SpawnMonster("Wolf", new Vector2(6, 15), monsters, allEntities);
        SpawnMonster("DarkMage", new Vector2(14, 14), monsters, allEntities);
    }

    private void SpawnMonster(string monsterId, Vector2 position, List<Monster> monsters, List<Entity> allEntities)
    {
        var data = MonsterDatabase.GetMonsterData(monsterId);
        var monster = new Monster(_whitePixel, position, monsterId, data);
        monsters.Add(monster);
        allEntities.Add(monster);
    }

    public void Update(GameTime gameTime, List<Monster> monsters)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var monster in monsters)
        {
            if (monster.IsDead)
            {
                if (monster.RespawnCooldownRemaining <= 0f)
                {
                    monster.RespawnCooldownRemaining = 5.0f; // Start cooldown
                }
                else
                {
                    monster.RespawnCooldownRemaining -= dt;
                    if (monster.RespawnCooldownRemaining <= 0f)
                    {
                        Respawn(monster);
                    }
                }
            }
        }
    }

    private void Respawn(Monster monster)
    {
        monster.CurrentHP = monster.MaxHP;
        monster.State = MonsterState.Idle;
        monster.Target = null;
        monster.MovementIntent = Vector2.Zero;
        
        // Randomize respawn position slightly, but keep it within bounds and walkable
        for (int i = 0; i < 10; i++)
        {
            int tx = (int)monster.WorldPosition.X + _rnd.Next(-2, 3);
            int ty = (int)monster.WorldPosition.Y + _rnd.Next(-2, 3);
            Vector2 randomPos = new Vector2(tx, ty);
            
            // Check if valid using tile map (basic bounds check)
            if (tx >= 0 && tx < _tileMap.Width && ty >= 0 && ty < _tileMap.Height)
            {
                if (_tileMap.GetTile(tx, ty).Type != TileType.Water && 
                    _tileMap.GetTile(tx, ty).Type != TileType.Blocked)
                {
                    // Optionally, we could check if another entity is there, 
                    // but for now, making it an exact integer prevents the float-equality bypass
                    monster.WorldPosition = randomPos;
                    break;
                }
            }
        }
    }
}
