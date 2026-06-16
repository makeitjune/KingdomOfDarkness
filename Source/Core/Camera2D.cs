using Microsoft.Xna.Framework;

namespace KingdomOfDarkness.Core;

public class Camera2D
{
    /// <summary>
    /// The camera's focus point in world-screen space.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// The center of the actual window/screen.
    /// </summary>
    public Vector2 ScreenCenter { get; set; }

    public Camera2D(int screenWidth, int screenHeight)
    {
        ScreenCenter = new Vector2(screenWidth / 2f, screenHeight / 2f);
        Position = Vector2.Zero;
    }

    /// <summary>
    /// Transforms world position to screen position, accounting for camera focus and window centering.
    /// </summary>
    public Vector2 WorldToCameraScreen(Vector2 worldPosition)
    {
        Vector2 screenPos = IsoMath.WorldToScreen(worldPosition);
        return screenPos - Position + ScreenCenter;
    }

    /// <summary>
    /// Gradually interpolates the camera position to follow a target screen position.
    /// </summary>
    public void FollowScreenPosition(Vector2 targetScreenPosition, float lerpFactor)
    {
        Position = Vector2.Lerp(Position, targetScreenPosition, lerpFactor);
    }

    /// <summary>
    /// Directly sets the camera position.
    /// </summary>
    public void LookAt(Vector2 targetWorldPosition)
    {
        Position = IsoMath.WorldToScreen(targetWorldPosition);
    }
}
