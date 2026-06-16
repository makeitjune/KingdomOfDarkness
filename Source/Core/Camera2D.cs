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
    public Vector2 ScreenCenter 
    {
        get 
        {
            float logicalWidth = GameConstants.VirtualWidth;
            float logicalHeight = GameConstants.VirtualHeight;
            // Shift the camera center up by half the HUD height so the player is centered in the visible area above the HUD
            float panelHeight = 144f;
            return new Vector2(logicalWidth / 2f, (logicalHeight - panelHeight) / 2f);
        }
    }

    public Camera2D(int screenWidth, int screenHeight)
    {
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

    /// <summary>
    /// Transforms screen position (e.g., mouse) to world position.
    /// </summary>
    public Vector2 ScreenToWorld(Vector2 screenPosition)
    {
        // 1. Convert screen position to camera-adjusted screen position
        Vector2 cameraAdjustedScreenPos = screenPosition + Position - ScreenCenter;
        
        // 2. Convert to world coordinates
        return IsoMath.ScreenToWorldApprox(cameraAdjustedScreenPos);
    }
}
