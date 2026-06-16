using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace KingdomOfDarkness.Core;

public class InputManager
{
    private KeyboardState _currentKeyboardState;
    private KeyboardState _previousKeyboardState;

    public Vector2 MovementIntent { get; private set; }
    public bool IsAttackRequested { get; private set; }

    public void Update()
    {
        _previousKeyboardState = _currentKeyboardState;
        _currentKeyboardState = Keyboard.GetState();

        UpdateMovementIntent();
        UpdateActions();
    }

    private void UpdateMovementIntent()
    {
        float dx = 0f;
        float dy = 0f;

        // W => worldY - 1 (moves toward upper-right screen diagonal)
        if (_currentKeyboardState.IsKeyDown(Keys.W))
        {
            dy -= 1f;
        }
        // S => worldY + 1 (moves toward lower-left screen diagonal)
        if (_currentKeyboardState.IsKeyDown(Keys.S))
        {
            dy += 1f;
        }
        // A => worldX - 1 (moves toward upper-left screen diagonal)
        if (_currentKeyboardState.IsKeyDown(Keys.A))
        {
            dx -= 1f;
        }
        // D => worldX + 1 (moves toward lower-right screen diagonal)
        if (_currentKeyboardState.IsKeyDown(Keys.D))
        {
            dx += 1f;
        }

        Vector2 intent = new Vector2(dx, dy);
        if (intent.LengthSquared() > 1f)
        {
            intent.Normalize();
        }

        MovementIntent = intent;
    }

    private void UpdateActions()
    {
        // Attack is requested if Space is pressed this frame but wasn't last frame, or is currently held.
        // Let's make it simple: Space key triggers attack.
        IsAttackRequested = _currentKeyboardState.IsKeyDown(Keys.Space);
    }

    public bool IsKeyPressed(Keys key)
    {
        return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
    }
}
