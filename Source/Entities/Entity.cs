using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;

namespace KingdomOfDarkness.Entities;

public abstract class Entity
{
    private static int _nextId = 1;

    public int Id { get; }
    public Vector2 WorldPosition { get; set; }
    public bool IsActive { get; set; } = true;
    public int DrawOrder { get; set; }

    protected Entity()
    {
        Id = _nextId++;
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch, Camera2D camera)
    {
    }
}
