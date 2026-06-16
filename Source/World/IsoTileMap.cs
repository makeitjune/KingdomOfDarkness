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
    private Texture2D _whitePixel;

    public IsoTileMap(GraphicsDevice graphicsDevice, int width = 40, int height = 40)
    {
        Width = width;
        Height = height;
        _tiles = new IsoTile[width, height];

        InitializeMap();
        GenerateTileTexture(graphicsDevice);

        _whitePixel = new Texture2D(graphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
    }

    private void InitializeMap()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                TileType type = TileType.Grass;

                // Create variety of terrain
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    type = TileType.Blocked; // map edge wall
                }
                else if (x >= 5 && x <= 15 && y >= 5 && y <= 8)
                {
                    type = TileType.Dirt; // Dirt patch
                }
                else if (x >= 20 && x <= 25 && y >= 20 && y <= 25)
                {
                    type = TileType.Sand; // Sand area
                }
                else if (x >= 25 && x <= 30 && y >= 8 && y <= 12)
                {
                    type = TileType.Stone; // Stone path
                }
                else if (x >= 12 && x <= 18 && y >= 28 && y <= 35)
                {
                    type = TileType.Water; // Lake
                }
                else if (x == 15 && y >= 10 && y <= 20)
                {
                    type = TileType.Blocked; // middle wall
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

    private void DrawLine(SpriteBatch sb, Vector2 p1, Vector2 p2, Color color, float thickness = 1.5f)
    {
        float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        float length = Vector2.Distance(p1, p2);
        sb.Draw(
            _whitePixel,
            p1,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness),
            SpriteEffects.None,
            0f
        );
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

                // Frustum culling: only draw if tile is within screen bounds (with padding)
                float cullPadding = 100f;
                if (screenPos.X < -cullPadding || screenPos.X > GameConstants.VirtualWidth + cullPadding ||
                    screenPos.Y < -cullPadding || screenPos.Y > GameConstants.VirtualHeight + cullPadding)
                {
                    continue;
                }

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

                // Overlay clearly visible border on blocked tiles
                if (tile.IsBlocked)
                {
                    float halfW = GameConstants.TileWidth / 2f;
                    float halfH = GameConstants.TileHeight / 2f;

                    Vector2 top = screenPos - new Vector2(0, halfH);
                    Vector2 bottom = screenPos + new Vector2(0, halfH);
                    Vector2 left = screenPos - new Vector2(halfW, 0);
                    Vector2 right = screenPos + new Vector2(halfW, 0);

                    // Red border for blocked wall, Cyan for water
                    Color borderCol = tile.Type == TileType.Water ? Color.Cyan : Color.Red;
                    float thickness = 1.5f;

                    DrawLine(spriteBatch, top, right, borderCol, thickness);
                    DrawLine(spriteBatch, right, bottom, borderCol, thickness);
                    DrawLine(spriteBatch, bottom, left, borderCol, thickness);
                    DrawLine(spriteBatch, left, top, borderCol, thickness);
                }
            }
        }
    }
}
