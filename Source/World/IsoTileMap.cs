using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;

namespace KingdomOfDarkness.World;

public class IsoTileMap
{
    private readonly IsoTile[,] _tiles;
    public int Width { get; }
    public int Height { get; }

    // Procedural textures
    private Texture2D _tileTexture;

    public IsoTileMap(GraphicsDevice graphicsDevice, int width = 20, int height = 20)
    {
        Width = width;
        Height = height;
        _tiles = new IsoTile[width, height];

        InitializeMap();
        GenerateTileTexture(graphicsDevice);
    }

    private void InitializeMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileType type = TileType.Ground;

                // Create some walls / water for visual variety and collision tests
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    type = TileType.Blocked; // map edge wall
                }
                else if (x == 5 && y >= 5 && y <= 10)
                {
                    type = TileType.Blocked; // middle wall
                }
                else if (x >= 12 && x <= 15 && y >= 12 && y <= 15)
                {
                    type = TileType.Water; // lake
                }

                _tiles[x, y] = new IsoTile(x, y, type);
            }
        }
    }

    private void GenerateTileTexture(GraphicsDevice gd)
    {
        int w = GameConstants.TileWidth;
        int h = GameConstants.TileHeight;
        _tileTexture = new Texture2D(gd, w, h);
        Color[] data = new Color[w * h];

        float cx = w / 2f - 0.5f;
        float cy = h / 2f - 0.5f;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float dx = Math.Abs(x - cx) / (w / 2f);
                float dy = Math.Abs(y - cy) / (h / 2f);
                float dist = dx + dy;

                int index = x + y * w;
                if (dist <= 1.0f)
                {
                    // If near the border, draw a dark grey border
                    if (dist >= 0.90f)
                    {
                        data[index] = new Color(40, 40, 40); // Dark border
                    }
                    else
                    {
                        data[index] = Color.White; // Filled diamond base (multiplied by debug color later)
                    }
                }
                else
                {
                    data[index] = Color.Transparent; // Outside the diamond
                }
            }
        }

        _tileTexture.SetData(data);
    }

    public IsoTile GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return null;
        return _tiles[x, y];
    }

    public void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
        // Origin of the tile draws at its center
        Vector2 origin = new Vector2(GameConstants.TileWidth / 2f, GameConstants.TileHeight / 2f);

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                IsoTile tile = _tiles[x, y];
                Vector2 worldPos = new Vector2(x, y);
                Vector2 screenPos = camera.WorldToCameraScreen(worldPos);

                spriteBatch.Draw(
                    _tileTexture,
                    screenPos,
                    null,
                    tile.DebugColor,
                    0f,
                    origin,
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
