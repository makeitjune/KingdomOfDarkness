using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KingdomOfDarkness.Core;
using KingdomOfDarkness.Entities;

namespace KingdomOfDarkness.Systems;

public class RenderOrderSystem
{
    /// <summary>
    /// Sorts active entities by logical depth (world X + Y) and renders them.
    /// </summary>
    public void DrawEntities(SpriteBatch spriteBatch, Camera2D camera, List<Entity> entities)
    {
        // Sort entities by their screen depth formula: depth = X + Y
        // Use Entity ID as a deterministic tie-breaker
        entities.Sort((a, b) =>
        {
            float depthA = a.WorldPosition.X + a.WorldPosition.Y;
            float depthB = b.WorldPosition.X + b.WorldPosition.Y;
            
            if (System.Math.Abs(depthA - depthB) > 0.0001f)
            {
                return depthA.CompareTo(depthB);
            }
            
            return a.Id.CompareTo(b.Id);
        });

        // Draw each active entity in sorted order
        foreach (var entity in entities)
        {
            if (entity.IsActive)
            {
                entity.Draw(spriteBatch, camera);
            }
        }
    }
}
